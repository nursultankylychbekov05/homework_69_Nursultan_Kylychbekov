using homeWork_54.DAL.Enums;

namespace homeWork_54.DAL.Entities;

public class TodoTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Assignee { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public TodoStatus Status { get; set; } = TodoStatus.New;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedDate { get; set; }
}