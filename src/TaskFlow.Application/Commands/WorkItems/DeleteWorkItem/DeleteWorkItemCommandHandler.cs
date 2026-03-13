using MediatR;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.WorkItems.DeleteWorkItem;

public class DeleteWorkItemCommandHandler : IRequestHandler<DeleteWorkItemCommand, bool>
{
    private readonly IWorkItemRepository _repository;

    public DeleteWorkItemCommandHandler(IWorkItemRepository repository)
        => _repository = repository;

    public async Task<bool> Handle(DeleteWorkItemCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if (entity == null)
        {
            return false;
        }

        await _repository.DeleteAsync(entity);
        await _repository.SaveChangesAsync();

        return true;
    }
}
