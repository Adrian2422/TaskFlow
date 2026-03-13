using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.WorkItems.UpdateWorkItem;

public class UpdateWorkItemCommandHandler : IRequestHandler<UpdateWorkItemCommand, WorkItemDto?>
{
    private readonly IWorkItemRepository _repository;

    public UpdateWorkItemCommandHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<WorkItemDto?> Handle(UpdateWorkItemCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if (entity == null)
        {
            return null;
        }

        if (request.Title != null) entity.Title = request.Title;
        if (request.Description != null) entity.Description = request.Description;

        await _repository.UpdateAsync(entity);
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
