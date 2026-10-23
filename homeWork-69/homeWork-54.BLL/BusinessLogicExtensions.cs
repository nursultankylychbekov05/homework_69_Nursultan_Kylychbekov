using homeWork_54.BLL.Interfaces;
using homeWork_54.BLL.Services;
using homeWork_54.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace homeWork_54.BLL;

public static class BusinessLogicExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services, string? connectionString)
    {
        services.AddDataAccess(connectionString);
        services.AddMemoryCache();
        services.AddScoped<ITaskService, TaskService>();
        return services;
    }
}