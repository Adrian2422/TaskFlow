using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Columns.UpdateColumn;

public class UpdateColumnCommandHandler : IRequestHandler<UpdateColumnCommand, ColumnDto?>
{
    private readonly IBoardColumnRepository _columnRepository;

    public UpdateColumnCommandHandler(IBoardColumnRepository columnRepository)
    {
        _columnRepository = columnRepository;
    }

    public async Task<ColumnDto?> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        var column = await _columnRepository.GetByIdAsync(request.ColumnId);
        if (column == null || column.BoardId != request.BoardId)
            return null;

        if (request.Name != null) column.Name = request.Name;
        if (request.Order.HasValue) column.Order = request.Order.Value;
        column.UpdatedAt = DateTime.UtcNow;

        await _columnRepository.UpdateAsync(column);
        await _columnRepository.SaveChangesAsync();

        return new ColumnDto
        {
            Id = column.Id,
            Name = column.Name,
            Order = column.Order,
            IsProtected = column.IsProtected
        };
    }
}
