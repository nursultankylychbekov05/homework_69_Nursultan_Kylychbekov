using homeWork_54.BLL.DTOs;
using homeWork_54.BLL.Interfaces;
using homeWork_54.DAL.Enums;
using homeWork_54.Models;
using Microsoft.AspNetCore.Mvc;

namespace homeWork_54.Controllers;

public class TasksController : Controller
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "searchTitle", "searchDescription", "dateFrom", "dateTo", "filterPriority", "filterStatus", "sortOrder", "page" })]
    public async Task<IActionResult> Index(
        string? searchTitle,
        string? searchDescription,
        DateTime? dateFrom,
        DateTime? dateTo,
        TaskPriority? filterPriority,
        TodoStatus? filterStatus,
        string sortOrder = "date_desc",
        int page = 1)
    {
        var result = await _taskService.GetTasksAsync(new TaskFilterDto
        {
            SearchTitle = searchTitle,
            SearchDescription = searchDescription,
            DateFrom = dateFrom,
            DateTo = dateTo,
            Priority = filterPriority,
            Status = filterStatus,
            SortOrder = sortOrder,
            Page = page
        });

        var viewModel = new TaskListViewModel
        {
            Tasks = result.Items.Select(ToModel).ToList(),
            SearchTitle = searchTitle,
            SearchDescription = searchDescription,
            DateFrom = dateFrom,
            DateTo = dateTo,
            FilterPriority = filterPriority,
            FilterStatus = filterStatus,
            CurrentSort = sortOrder,
            PageNumber = result.Page,
            TotalPages = result.TotalPages
        };

        return View(viewModel);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoTask task)
    {
        if (!ModelState.IsValid)
            return View(task);

        await _taskService.CreateAsync(new CreateTaskDto
        {
            Title = task.Title,
            Description = task.Description,
            Assignee = task.Assignee,
            Priority = task.Priority
        });

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Open(int id)
    {
        await _taskService.OpenAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(int id)
    {
        await _taskService.CloseAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _taskService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var dto = await _taskService.GetByIdAsync(id);
        if (dto == null) return NotFound();

        return View(ToModel(dto));
    }

    private static TodoTask ToModel(TaskDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        Description = dto.Description,
        Assignee = dto.Assignee,
        Priority = dto.Priority,
        Status = dto.Status,
        CreatedDate = dto.CreatedDate,
        ClosedDate = dto.ClosedDate
    };
}