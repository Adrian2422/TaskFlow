using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Infrastructure.AppDbContext;
using TaskFlow.Infrastructure.BackgroundJobs;
using TaskFlow.Infrastructure.Persistence;
using TaskFlow.Infrastructure.Services;

namespace TaskFlow.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        services.AddScoped<IWorkItemRepository, WorkItemRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
        services.AddScoped<DatabaseSeeder>();

        services.AddScoped<IOrderNormalizationService, OrderNormalizationService>();
        services.AddHostedService<WorkItemNormalizationWorker>();
        services.AddHostedService<ColumnNormalizationWorker>();

        return services;
    }
}