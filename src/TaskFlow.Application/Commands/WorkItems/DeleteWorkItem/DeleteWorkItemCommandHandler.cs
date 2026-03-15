using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.WorkItems.DeleteWorkItem;

public class DeleteWorkItemCommandHandler : IRequestHandler<DeleteWorkItemCommand, Result>
{
    private readonly IWorkItemRepository _repository;

    public DeleteWorkItemCommandHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<Result> Handle(DeleteWorkItemCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if (entity == null)
        {
            return Result.Failure(WorkItemErrors.NotFound(request.Id));
        }

        if (!entity.IsArchived)
        {
            return Result.Failure(WorkItemErrors.DeleteNotArchived);
        }

        await _repository.DeleteAsync(entity);
        await _repository.SaveChangesAsync();

        return Result.Success();
    }
}
