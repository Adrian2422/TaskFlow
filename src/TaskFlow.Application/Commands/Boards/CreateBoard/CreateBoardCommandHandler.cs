using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Boards.CreateBoard;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, Result<BoardDto>>
{
    private readonly IBoardRepository _boardRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateBoardCommandHandler(IBoardRepository boardRepository, ICurrentUserService currentUserService)
    {
        _boardRepository = boardRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BoardDto>> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.UserId.HasValue)
        {
            return Result.Failure<BoardDto>(new TaskFlow.Domain.Common.Error("Auth.Unauthorized", "User not authenticated"));
        }

        var userId = _currentUserService.UserId.Value;
        var board = Board.Create(request.Name, request.Description, userId);
        
        // Add creator as Owner
        board.Members.Add(new BoardMember
        {
            BoardId = board.Id,
            UserId = userId,
            Role = BoardRole.Owner
        });

        var newColumn = BoardColumn.Create("To Do", 1000, board.Id, isProtected: true);
        board.Columns.Add(newColumn);

        await _boardRepository.CreateAsync(board);
        await _boardRepository.SaveChangesAsync();

        return new BoardDto
        {
            Id = board.Id,
            Name = board.Name,
            Description = board.Description
        };
    }
}
