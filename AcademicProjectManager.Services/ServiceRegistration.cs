using AcademicProjectManager.Services.Implementations;
using AcademicProjectManager.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AcademicProjectManager.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddAcademicProjectServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IMemberService, MemberService>();

        return services;
    }
}
