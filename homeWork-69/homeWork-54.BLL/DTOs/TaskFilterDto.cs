using homeWork_54.DAL.Enums;

namespace homeWork_54.BLL.DTOs;

public class TaskFilterDto
{
    public string? SearchTitle { get; set; }
    public string? SearchDescription { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public TaskPriority? Priority { get; set; }
    public TodoStatus? Status { get; set; }
    public string SortOrder { get; set; } = "date_desc";
    public int Page { get; set; } = 1;
}