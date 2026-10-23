using homeWork_54.DAL.Enums;

namespace homeWork_54.BLL.DTOs;

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Assignee { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; }
    public TodoStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ClosedDate { get; set; }
}