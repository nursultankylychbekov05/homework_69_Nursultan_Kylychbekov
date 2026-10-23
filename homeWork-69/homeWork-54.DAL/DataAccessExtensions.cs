using homeWork_54.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace homeWork_54.DAL;

public static class DataAccessExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<TodoContext>(options =>
            options.UseNpgsql(connectionString,
                b => b.MigrationsAssembly(typeof(TodoContext).Assembly.GetName().Name)));
        services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
        return services;
    }

    public static void ApplyMigrations(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        scope.ServiceProvider.GetRequiredService<TodoContext>().Database.Migrate();
    }
}