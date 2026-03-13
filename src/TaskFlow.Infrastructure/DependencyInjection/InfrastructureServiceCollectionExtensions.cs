using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Infrastructure.BackgroundJobs;
using TaskFlow.Infrastructure.Persistence;
using TaskFlow.Infrastructure.Services;

namespace TaskFlow.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IWorkItemRepository, WorkItemRepository>();
        services.AddScoped<DatabaseSeeder>();

        services.AddScoped<IOrderNormalizationService, OrderNormalizationService>();
        services.AddHostedService<OrderNormalizationWorker>();
        
        return services;
    }
}