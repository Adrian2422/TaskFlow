using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Infrastructure.BackgroundJobs;

public class ColumnNormalizationWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ColumnNormalizationWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const double minGap = 0.0001;

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);

            using var scope = _scopeFactory.CreateScope();

            var service = scope.ServiceProvider
                .GetRequiredService<IOrderNormalizationService>();

            await service.NormalizeColumnsIfNeeded(minGap);
        }
    }
}