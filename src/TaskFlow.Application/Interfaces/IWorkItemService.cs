using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;


public interface IWorkItemService
{
    Task<PagedResult<WorkItemDto>> GetPagedAsync(PaginationQuery query);
    Task<List<WorkItemDto>> GetAllAsync();
    Task<WorkItemDto?> GetByIdAsync(Guid id);
    Task<WorkItemDto> CreateAsync(CreateWorkItemDto dto);
    Task<WorkItemDto?> UpdateAsync(Guid id, UpdateWorkItemDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task Move(Guid id, MoveWorkItemDto dto);
}