using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.WorkItems.MoveWorkItem;

public class MoveWorkItemCommandHandler : IRequestHandler<MoveWorkItemCommand, Result>
{
    private readonly IWorkItemRepository _repository;

    public MoveWorkItemCommandHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<Result> Handle(MoveWorkItemCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            return Result.Failure(WorkItemErrors.NotFound(request.Id));
        }

        double newOrder;
        if (request.PrevPosition.HasValue && request.NextPosition.HasValue)
            newOrder = (request.PrevPosition.Value + request.NextPosition.Value) / 2.0;
        else if (request.PrevPosition.HasValue)
            newOrder = request.PrevPosition.Value + 1000;
        else if (request.NextPosition.HasValue)
            newOrder = request.NextPosition.Value / 2.0;
        else
            newOrder = 1000;

        entity.ColumnId = request.ColumnId;
        entity.Order = newOrder;

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();

        return Result.Success();
    }
}
