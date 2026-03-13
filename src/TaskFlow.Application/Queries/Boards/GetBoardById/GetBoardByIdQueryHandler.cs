using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Queries.Boards.GetBoardById;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, BoardDetailDto?>
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardByIdQueryHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<BoardDetailDto?> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetWithColumnsAndItemsAsync(request.Id);
        if (board == null) return null;

        return new BoardDetailDto
        {
            Id = board.Id,
            Name = board.Name,
            Description = board.Description,
            Columns = board.Columns
                .OrderBy(c => c.Order)
                .Select(c => new ColumnDetailDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Order = c.Order,
                    IsProtected = c.IsProtected,
                    WorkItems = c.WorkItems
                        .OrderBy(w => w.Order)
                        .Select(w => new WorkItemDto
                        {
                            Id = w.Id,
                            Title = w.Title,
                            Description = w.Description,
                            Order = w.Order
                        }).ToList()
                }).ToList()
        };
    }
}
