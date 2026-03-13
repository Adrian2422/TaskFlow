using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Services;

namespace TaskFlow.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IWorkItemService, WorkItemService>();
        services.AddScoped<IBoardService, BoardService>();
        services.AddScoped<IBoardColumnService, BoardColumnService>();

        services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();

        return services;
    }
}