using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.WorkItems.RestoreWorkItem;

public class RestoreWorkItemCommandHandler : IRequestHandler<RestoreWorkItemCommand, Result>
{
    private readonly IWorkItemRepository _repository;

    public RestoreWorkItemCommandHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<Result> Handle(RestoreWorkItemCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if (entity == null)
        {
            return Result.Failure(WorkItemErrors.NotFound(request.Id));
        }

        entity.Restore();

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();

        return Result.Success();
    }
}
