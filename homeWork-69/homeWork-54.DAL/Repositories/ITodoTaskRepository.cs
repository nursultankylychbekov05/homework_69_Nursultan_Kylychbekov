using homeWork_54.DAL.Entities;

namespace homeWork_54.DAL.Repositories;

public interface ITodoTaskRepository
{
    Task<(List<TodoTask> Items, int TotalCount)> GetPagedAsync(TaskFilter filter, string sortOrder, int page, int pageSize);
    Task<TodoTask?> GetByIdAsync(int id);
    Task AddAsync(TodoTask task);
    Task UpdateAsync(TodoTask task);
    Task DeleteAsync(TodoTask task);
}