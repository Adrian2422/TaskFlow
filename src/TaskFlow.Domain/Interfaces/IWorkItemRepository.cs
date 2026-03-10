using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces;

public interface IWorkItemRepository
{
    Task<List<WorkItem>> GetAllAsync();
    Task<WorkItem?> GetByIdAsync(Guid id);
    Task CreateAsync(WorkItem item);
    Task UpdateAsync(WorkItem item);
    Task DeleteAsync(WorkItem item);
    Task SaveChangesAsync();
}