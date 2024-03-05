namespace TaskManagementAPI
{
    public interface ITaskService
    {
        Task<IEnumerable<Task>> Get();
        Task<Task> Save(Task task);
        Task<Task> Update(Guid id, Task task);
        void Remove(Guid id);
    }
}