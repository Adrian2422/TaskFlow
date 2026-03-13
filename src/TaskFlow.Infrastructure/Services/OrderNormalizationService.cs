using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Infrastructure.AppDbContext;

namespace TaskFlow.Infrastructure.Services;

public class OrderNormalizationService : IOrderNormalizationService
{
    public readonly ApplicationDbContext _context;

    public OrderNormalizationService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task NormalizeColumnsIfNeeded(double minGap)
    {
        var columnIds = await _context.WorkItems
            .Select(x => x.ColumnId)
            .Distinct()
            .ToListAsync();

        foreach (var columnId in columnIds)
        {
            if (await NeedsNormalization(columnId, minGap))
            {
                await NormalizeColumn(columnId);
            }
        }
    }
    
    private async Task<bool> NeedsNormalization(Guid columnId, double minGap)
    {
        var order = await _context.WorkItems
            .Where(x => x.ColumnId == columnId)
            .OrderBy(x => x.Order)
            .Select(x => x.Order)
            .ToListAsync();

        for (int i = 1; i < order.Count; i++)
        {
            if (order[i] - order[i - 1] < minGap)
                return true;
        }

        return false;
    }

    private async Task NormalizeColumn(Guid columnId)
    {
        var items = await _context.WorkItems
            .Where(x => x.ColumnId == columnId)
            .OrderBy(x => x.Order)
            .ToListAsync();

        double order = 1000;

        foreach (var item in items)
        {
            item.Order = order;
            order += 1000;
        }

        await _context.SaveChangesAsync();
    }
}
