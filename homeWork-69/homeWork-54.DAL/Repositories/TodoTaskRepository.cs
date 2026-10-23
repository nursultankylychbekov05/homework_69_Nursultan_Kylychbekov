using homeWork_54.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace homeWork_54.DAL.Repositories;

public class TodoTaskRepository : ITodoTaskRepository
{
    private readonly TodoContext _context;

    public TodoTaskRepository(TodoContext context) => _context = context;

    public async Task<(List<TodoTask> Items, int TotalCount)> GetPagedAsync(
        TaskFilter filter, string sortOrder, int page, int pageSize)
    {
        IQueryable<TodoTask> query = _context.Tasks.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(t => t.Title.ToLower().Contains(filter.Title.ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.Description))
            query = query.Where(t => t.Description != null && t.Description.ToLower().Contains(filter.Description.ToLower()));

        if (filter.DateFrom.HasValue)
            query = query.Where(t => t.CreatedDate >= filter.DateFrom.Value.Date);

        if (filter.DateTo.HasValue)
            query = query.Where(t => t.CreatedDate <= filter.DateTo.Value.Date.AddDays(1).AddTicks(-1));

        if (filter.Priority.HasValue)
            query = query.Where(t => t.Priority == filter.Priority.Value);

        if (filter.Status.HasValue)
            query = query.Where(t => t.Status == filter.Status.Value);

        query = sortOrder switch
        {
            "title_asc" => query.OrderBy(t => t.Title),
            "title_desc" => query.OrderByDescending(t => t.Title),
            "priority_asc" => query.OrderBy(t => t.Priority),
            "priority_desc" => query.OrderByDescending(t => t.Priority),
            "status_asc" => query.OrderBy(t => t.Status),
            "status_desc" => query.OrderByDescending(t => t.Status),
            "date_asc" => query.OrderBy(t => t.CreatedDate),
            _ => query.OrderByDescending(t => t.CreatedDate)
        };

        int total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, total);
    }

    public Task<TodoTask?> GetByIdAsync(int id) => _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(TodoTask task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TodoTask task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TodoTask task)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}