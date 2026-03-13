using MediatR;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Columns.DeleteColumn;

public class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand, bool>
{
    private readonly IBoardColumnRepository _columnRepository;

    public DeleteColumnCommandHandler(IBoardColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<bool> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        var column = await _columnRepository.GetByIdAsync(request.ColumnId);
        if (column == null || column.BoardId != request.BoardId)
            return false;

        if (column.IsProtected)
            throw new InvalidOperationException("Cannot delete a protected column.");

        await _columnRepository.DeleteAsync(column);
        await _columnRepository.SaveChangesAsync();
        return true;
    }
}
