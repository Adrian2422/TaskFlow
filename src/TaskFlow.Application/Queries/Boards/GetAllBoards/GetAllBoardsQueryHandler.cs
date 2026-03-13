using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Queries.Boards.GetAllBoards;

public class GetAllBoardsQueryHandler : IRequestHandler<GetAllBoardsQuery, List<BoardDto>>
{
    private readonly IBoardRepository _boardRepository;

    public GetAllBoardsQueryHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<List<BoardDto>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
    {
        var boards = await _boardRepository.GetAllAsync();
        return boards.Select(b => new BoardDto
        {
            Id = b.Id,
            Name = b.Name,
            Description = b.Description
        }).ToList();
    }
}
