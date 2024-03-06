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

        public async Task<Task> Save(TaskView taskNew)
        {
            var taskToSave = new Task()
            {
                Name = taskNew.Name,
                Description = taskNew.Description,
                Icon = taskNew.Icon,
                Status = taskNew.Status,
                Deadline = taskNew.Deadline
            };


            taskToSave.IdTask = Guid.NewGuid();
            taskToSave.CreationDate = DateTime.UtcNow;

            using (var transaction = await _taskManagementContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _taskManagementContext.Tasks.AddAsync(taskToSave);
                    await _taskManagementContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return taskToSave;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Error saving task", ex);
                }
            }
        }

        public async Task<Task> Update(Guid id, TaskView updatedTask)
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

        public async Task<Task> Remove(Guid id)
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
                        return existingTask; // Devuelve la tarea eliminada
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
