using homeWork_54.BLL.DTOs;

namespace homeWork_54.BLL.Interfaces;

public interface ITaskService
{
    Task<PagedResult<TaskDto>> GetTasksAsync(TaskFilterDto filter);
    Task<TaskDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateTaskDto dto);
    Task<bool> OpenAsync(int id);
    Task<bool> CloseAsync(int id);
    Task<bool> DeleteAsync(int id);
}