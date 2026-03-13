using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Infrastructure.AppDbContext;

namespace TaskFlow.Infrastructure.Persistence;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly ApplicationDbContext _context;

    public WorkItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkItem>> GetAllAsync()
    {
        return await _context.WorkItems.ToListAsync();
    }

    public async Task<(List<WorkItem> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _context.WorkItems.OrderBy(w => w.Order);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<WorkItem?> GetByIdAsync(Guid id)
    {
        return await _context.WorkItems.FindAsync(id);
    }

    public async Task<double?> GetMaxOrderInColumnAsync(Guid columnId)
    {
        return await _context.WorkItems
            .Where(w => w.ColumnId == columnId)
            .MaxAsync(w => (double?)w.Order);
    }

    public async Task<double?> GetMaxOrderInBacklogAsync(Guid boardId)
    {
        return await _context.WorkItems
            .Where(w => w.BoardId == boardId && w.ColumnId == null)
            .MaxAsync(w => (double?)w.Order);
    }

    public async Task CreateAsync(WorkItem item)
    {
        await _context.WorkItems.AddAsync(item);
    }

    public async Task UpdateAsync(WorkItem item)
    {
        _context.WorkItems.Update(item);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(WorkItem item)
    {
        _context.WorkItems.Remove(item);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}