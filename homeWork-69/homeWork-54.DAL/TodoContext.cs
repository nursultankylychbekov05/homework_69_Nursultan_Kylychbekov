using homeWork_54.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace homeWork_54.DAL;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

    public DbSet<TodoTask> Tasks { get; set; }
}