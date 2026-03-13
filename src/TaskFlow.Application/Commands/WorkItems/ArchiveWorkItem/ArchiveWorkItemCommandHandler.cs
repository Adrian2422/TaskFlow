using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.WorkItems.ArchiveWorkItem;

public class ArchiveWorkItemCommandHandler : IRequestHandler<ArchiveWorkItemCommand, Result>
{
    private readonly IWorkItemRepository _repository;

    public ArchiveWorkItemCommandHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<Result> Handle(ArchiveWorkItemCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if (entity == null)
        {
            return Result.Failure(WorkItemErrors.NotFound(request.Id));
        }

        entity.Archive();

        await _repository.UpdateAsync(entity);
        await _repository.SaveChangesAsync();

        return Result.Success();
    }
}
