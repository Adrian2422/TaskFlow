using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Columns.CreateColumn;

public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, Result<ColumnDto>>
{
    private readonly IBoardColumnRepository _columnRepository;
    private readonly IBoardRepository _boardRepository;

    public CreateColumnCommandHandler(IBoardColumnRepository columnRepository, IBoardRepository boardRepository)
    {
        _columnRepository = columnRepository;
        _boardRepository = boardRepository;
    }

    public async Task<Result<ColumnDto>> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.BoardId);
        if (board == null)
        {
            return Result.Failure<ColumnDto>(BoardErrors.NotFound(request.BoardId));
        }

        var maxOrder = await _columnRepository.GetMaxOrderInBoardAsync(request.BoardId);
        var order = maxOrder.HasValue ? maxOrder.Value + 1000 : 0;
        var column = BoardColumn.Create(request.Name, order, request.BoardId);

        await _columnRepository.CreateAsync(column);
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
