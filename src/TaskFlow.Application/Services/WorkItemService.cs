using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Services;

public class WorkItemService : IWorkItemService
{
    private readonly IWorkItemRepository _repository;

    public WorkItemService(IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<WorkItemDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(ToDto).ToList();
    }

    public async Task<WorkItemDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : ToDto(entity);
    }

    public async Task<WorkItemDto> CreateAsync(CreateWorkItemDto dto)
    {
        var entity = WorkItem.Create(dto.Title, dto.Description, dto.ColumnId);

        var maxOrder = await _repository.GetMaxOrderInColumnAsync(dto.ColumnId);
        entity.Order = maxOrder.HasValue ? maxOrder.Value + 1000 : 1000;

        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.CreateAsync(entity);
        await _repository.SaveChangesAsync();

        return ToDto(entity);
    }

    public async Task<WorkItemDto?> UpdateAsync(Guid id, UpdateWorkItemDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
        {
            return null;
        }

        if (dto.Title != null) entity.Title = dto.Title;
        if (dto.Description != null) entity.Description = dto.Description;

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();

        return ToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
        {
            return false;
        }

        await _repository.DeleteAsync(entity);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task Move(Guid id, MoveWorkItemDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            return;
        }

        double newOrder;
        if (dto.PrevPosition.HasValue && dto.NextPosition.HasValue)
            newOrder = (dto.PrevPosition.Value + dto.NextPosition.Value) / 2.0;
        else if (dto.PrevPosition.HasValue)
            newOrder = dto.PrevPosition.Value + 1000;
        else if (dto.NextPosition.HasValue)
            newOrder = dto.NextPosition.Value / 2.0;
        else
            newOrder = 1000;

        entity.ColumnId = dto.ColumnId;
        entity.Order = newOrder;

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();
    }

    private static WorkItemDto ToDto(WorkItem x) => new()
    {
        Id = x.Id,
        Title = x.Title,
        Description = x.Description
    };
}