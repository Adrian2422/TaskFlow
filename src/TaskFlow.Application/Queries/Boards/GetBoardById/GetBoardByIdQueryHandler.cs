using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Queries.Boards.GetBoardById;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, Result<BoardDetailDto>>
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardByIdQueryHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result<BoardDetailDto>> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetWithColumnsAndItemsAsync(request.Id);
        if (board == null)
        {
            return Result.Failure<BoardDetailDto>(BoardErrors.NotFound(request.Id));
        }

        return new BoardDetailDto
        {
            Id = board.Id,
            Name = board.Name,
            Description = board.Description,
            Backlog = board.BacklogItems
                .Where(w => !w.IsArchived)
                .OrderBy(w => w.Order)
                .Select(w => new WorkItemDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    Description = w.Description,
                    Order = w.Order
                }).ToList(),
            Columns = board.Columns
                .OrderBy(c => c.Order)
                .Select(c => new ColumnDetailDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Order = c.Order,
                    IsProtected = c.IsProtected,
                    WorkItems = c.WorkItems
                        .Where(w => !w.IsArchived)
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
