using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Infrastructure.AppDbContext;

namespace TaskFlow.Infrastructure.Services;

public class OrderNormalizationService : IOrderNormalizationService
{
    private readonly ApplicationDbContext _context;

    public OrderNormalizationService(ApplicationDbContext context)
    {
        _context = context;
    }

    private async Task<bool> WorkItemsNeedsNormalization(Guid columnId, double minGap)
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

    private async Task<bool> ColumnsNeedsNormalization(Guid boardId, double minGap)
    {
        var order = await _context.Columns
            .Where(x => x.BoardId == boardId)
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

    public async Task NormalizeWorkItemsIfNeeded(double minGap)
    {
        var columnIds = await _context.WorkItems
            .Select(x => x.ColumnId)
            .Distinct()
            .ToListAsync();

        foreach (var columnId in columnIds)
        {
            if (await WorkItemsNeedsNormalization(columnId, minGap))
            {
                await NormalizeWorkItems(columnId);
            }
        }
    }

    public async Task NormalizeColumnsIfNeeded(double minGap)
    {
        var boardIds = await _context.Columns
            .Select(x => x.BoardId)
            .Distinct()
            .ToListAsync();

        foreach (var boardId in boardIds)
        {
            if (await ColumnsNeedsNormalization(boardId, minGap))
            {
                await NormalizeColumns(boardId);
            }
        }
    }

    private async Task NormalizeWorkItems(Guid columnId)
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

    private async Task NormalizeColumns(Guid boardId)
    {
        var items = await _context.Columns
            .Where(x => x.BoardId == boardId)
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
