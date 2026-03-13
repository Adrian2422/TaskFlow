using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Queries.Boards.GetAllBoards;

public class GetAllBoardsQueryHandler : IRequestHandler<GetAllBoardsQuery, Result<List<BoardDto>>>
{
    private readonly IBoardRepository _boardRepository;

    public GetAllBoardsQueryHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result<List<BoardDto>>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
    {
        var boards = await _boardRepository.GetAllAsync();

        var result = boards
            .Where(b => !b.IsArchived)
            .Select(b => new BoardDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description
            }).ToList();

        return Result.Success(result);
    }
}
