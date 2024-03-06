namespace TaskManagementAPI
{
    public interface ITaskService
    {
        Task<IEnumerable<Task>> Get();
        Task<Task> Save(TaskView task);
        Task<Task> Update(Guid id, TaskView task);
        Task<Task> Remove(Guid id);
    }
}