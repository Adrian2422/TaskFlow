using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.WorkItems.CreateWorkItem;

public class CreateWorkItemCommandHandler : IRequestHandler<CreateWorkItemCommand, WorkItemDto>
{
    private readonly IWorkItemRepository _repository;

    public CreateWorkItemCommandHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<WorkItemDto> Handle(CreateWorkItemCommand request, CancellationToken ct)
    {
        var maxOrder = await _repository.GetMaxOrderInColumnAsync(request.ColumnId);
        var order = maxOrder.HasValue ? maxOrder.Value + 1000 : 1000;

        var entity = WorkItem.Create(request.Title, request.Description, request.ColumnId, order);
        entity.UpdatedAt = DateTime.UtcNow;

        await _repository.CreateAsync(entity);
        await _repository.SaveChangesAsync();

        return new WorkItemDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Order = entity.Order
        };
    }
}
