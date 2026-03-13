using MediatR;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Columns.MoveColumn;

public class MoveColumnCommandHandler : IRequestHandler<MoveColumnCommand>
{
    private readonly IBoardColumnRepository _columnRepository;

    public MoveColumnCommandHandler(IBoardColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task Handle(MoveColumnCommand request, CancellationToken cancellationToken)
    {
        var column = await _columnRepository.GetByIdAsync(request.ColumnId);
        if (column == null || column.BoardId != request.BoardId) return;

        double newOrder;
        if (request.PrevPosition.HasValue && request.NextPosition.HasValue)
            newOrder = (request.PrevPosition.Value + request.NextPosition.Value) / 2.0;
        else if (request.PrevPosition.HasValue)
            newOrder = request.PrevPosition.Value + 1000;
        else if (request.NextPosition.HasValue)
            newOrder = request.NextPosition.Value / 2.0;
        else
            newOrder = 1000;

        column.Order = newOrder;
        await _columnRepository.UpdateAsync(column);
        await _columnRepository.SaveChangesAsync();
    }
}
