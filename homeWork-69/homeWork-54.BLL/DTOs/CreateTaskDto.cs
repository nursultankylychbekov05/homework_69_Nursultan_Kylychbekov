using homeWork_54.DAL.Enums;

namespace homeWork_54.BLL.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Assignee { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; }
}