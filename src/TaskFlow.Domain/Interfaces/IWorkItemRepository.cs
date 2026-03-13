using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces;

public interface IWorkItemRepository
{
    Task<List<WorkItem>> GetAllAsync();
    Task<(List<WorkItem> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    Task<WorkItem?> GetByIdAsync(Guid id);
    Task<double?> GetMaxOrderInColumnAsync(Guid columnId);
    Task CreateAsync(WorkItem item);
    Task UpdateAsync(WorkItem item);
    Task DeleteAsync(WorkItem item);
    Task SaveChangesAsync();
}