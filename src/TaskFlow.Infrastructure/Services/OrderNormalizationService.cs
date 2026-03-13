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

    private async Task<bool> WorkItemsNeedsNormalization(Guid? columnId, Guid boardId, double minGap)
    {
        var order = await _context.WorkItems
            .Where(x => x.ColumnId == columnId && x.BoardId == boardId)
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
        var groups = await _context.WorkItems
            .Select(x => new { x.ColumnId, x.BoardId })
            .Distinct()
            .ToListAsync();

        foreach (var group in groups)
        {
            if (await WorkItemsNeedsNormalization(group.ColumnId, group.BoardId, minGap))
            {
                await NormalizeWorkItems(group.ColumnId, group.BoardId);
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

    private async Task NormalizeWorkItems(Guid? columnId, Guid boardId)
    {
        var items = await _context.WorkItems
            .Where(x => x.ColumnId == columnId && x.BoardId == boardId)
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
