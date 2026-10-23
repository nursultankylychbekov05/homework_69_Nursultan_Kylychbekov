using homeWork_54.DAL.Enums;

namespace homeWork_54.DAL.Repositories;

public class TaskFilter
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public TaskPriority? Priority { get; set; }
    public TodoStatus? Status { get; set; }
}