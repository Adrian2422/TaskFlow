using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Columns.DeleteColumn;

public class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand, Result>
{
    private readonly IBoardColumnRepository _columnRepository;
    private readonly IWorkItemRepository _workItemRepository;

    public DeleteColumnCommandHandler(IBoardColumnRepository columnRepository, IWorkItemRepository workItemRepository)
    {
        _columnRepository = columnRepository;
        _workItemRepository = workItemRepository;
    }

    public async Task<Result> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        var column = await _columnRepository.GetByIdWithWorkItemsAsync(request.ColumnId);
        if (column == null || column.BoardId != request.BoardId)
        {
            return Result.Failure(ColumnErrors.NotFound(request.ColumnId, request.BoardId));
        }

        if (column.IsProtected)
        {
            return Result.Failure(ColumnErrors.Protected);
        }

        var maxOrder = await _workItemRepository.GetMaxOrderInBacklogAsync(request.BoardId);
        var currentOrder = maxOrder.HasValue ? maxOrder.Value + 1000 : 1000;

        foreach (var item in column.WorkItems)
        {
            item.ColumnId = null;
            item.Order = currentOrder;
            currentOrder += 1000;
            await _workItemRepository.UpdateAsync(item);
        }

        await _columnRepository.DeleteAsync(column);
        await _columnRepository.SaveChangesAsync();
        await _workItemRepository.SaveChangesAsync();

        return Result.Success();
    }
}
