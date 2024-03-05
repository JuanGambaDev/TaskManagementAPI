using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementAPI
{
    public class TaskService : ITaskService
    {
        private readonly TaskManagementContext _taskManagementContext;

        public TaskService(TaskManagementContext taskManagementContext)
        {
            _taskManagementContext = taskManagementContext;
        }

        public async Task<IEnumerable<Task>> Get()
        {
            return await _taskManagementContext.Tasks.ToListAsync();
        }

        public async Task<Task> Save(Task taskNew)
        {
            taskNew.IdTask = Guid.NewGuid();
            taskNew.CreationDate = DateTime.UtcNow;

            using (var transaction = await _taskManagementContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _taskManagementContext.Tasks.AddAsync(taskNew);
                    await _taskManagementContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return taskNew;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Error saving task", ex);
                }
            }
        }

        public async Task<Task> Update(Guid id, Task updatedTask)
        {
            using (var transaction = await _taskManagementContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingTask = await _taskManagementContext.Tasks.FindAsync(id);

                    if (existingTask != null)
                    {
                        existingTask.Name = updatedTask.Name;
                        existingTask.Description = updatedTask.Description;
                        existingTask.Icon = updatedTask.Icon;
                        existingTask.Status = updatedTask.Status;
                        existingTask.Deadline = updatedTask.Deadline;

                        await _taskManagementContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return existingTask;
                    }
                    else
                    {
                        throw new Exception("Task not found");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Error updating task", ex);
                }
            }
        }

        public async void Remove(Guid id)
        {
            using (var transaction = await _taskManagementContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingTask = await _taskManagementContext.Tasks.FindAsync(id);

                    if (existingTask != null)
                    {
                        _taskManagementContext.Tasks.Remove(existingTask);
                        await _taskManagementContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        throw new Exception("Task not found");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Error removing task", ex);
                }
            }
        }
    }
}
