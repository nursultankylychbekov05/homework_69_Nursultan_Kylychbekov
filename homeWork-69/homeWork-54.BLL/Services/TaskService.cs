using homeWork_54.BLL.DTOs;
using homeWork_54.BLL.Interfaces;
using homeWork_54.DAL.Entities;
using homeWork_54.DAL.Enums;
using homeWork_54.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace homeWork_54.BLL.Services;

public class TaskService : ITaskService
{
    private const int PageSize = 10;
    private static int _cacheVersion;

    private readonly ITodoTaskRepository _repository;
    private readonly IMemoryCache _cache;

    public TaskService(ITodoTaskRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<PagedResult<TaskDto>> GetTasksAsync(TaskFilterDto f)
    {
        int page = Math.Max(1, f.Page);
        string key = $"tasks_v{_cacheVersion}_{f.SearchTitle}_{f.SearchDescription}_{f.DateFrom}_{f.DateTo}_{f.Priority}_{f.Status}_{f.SortOrder}_{page}";

        if (_cache.TryGetValue(key, out PagedResult<TaskDto>? cached) && cached != null)
            return cached;

        var filter = new TaskFilter
        {
            Title = f.SearchTitle,
            Description = f.SearchDescription,
            DateFrom = f.DateFrom,
            DateTo = f.DateTo,
            Priority = f.Priority,
            Status = f.Status
        };

        var (items, total) = await _repository.GetPagedAsync(filter, f.SortOrder, page, PageSize);

        var result = new PagedResult<TaskDto>
        {
            Items = items.Select(ToDto).ToList(),
            TotalCount = total,
            Page = page,
            PageSize = PageSize
        };

        _cache.Set(key, result, TimeSpan.FromMinutes(2));
        return result;
    }

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        return task == null ? null : ToDto(task);
    }

    public async Task CreateAsync(CreateTaskDto dto)
    {
        var task = new TodoTask
        {
            Title = dto.Title,
            Description = dto.Description,
            Assignee = dto.Assignee,
            Priority = dto.Priority,
            Status = TodoStatus.New,
            CreatedDate = DateTime.UtcNow
        };
        await _repository.AddAsync(task);
        InvalidateCache();
    }

    public async Task<bool> OpenAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null || task.Status != TodoStatus.New) return false;

        task.Status = TodoStatus.Opened;
        await _repository.UpdateAsync(task);
        InvalidateCache();
        return true;
    }

    public async Task<bool> CloseAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null || task.Status != TodoStatus.Opened) return false;

        task.Status = TodoStatus.Closed;
        task.ClosedDate = DateTime.UtcNow;
        await _repository.UpdateAsync(task);
        InvalidateCache();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null || task.Status == TodoStatus.Opened) return false;

        await _repository.DeleteAsync(task);
        InvalidateCache();
        return true;
    }

    private static void InvalidateCache() => Interlocked.Increment(ref _cacheVersion);

    private static TaskDto ToDto(TodoTask t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Description = t.Description,
        Assignee = t.Assignee,
        Priority = t.Priority,
        Status = t.Status,
        CreatedDate = t.CreatedDate,
        ClosedDate = t.ClosedDate
    };
}