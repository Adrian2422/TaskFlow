using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Boards.CreateBoard;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, BoardDto>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardColumnRepository _columnRepository;

    public CreateBoardCommandHandler(IBoardRepository boardRepository, IBoardColumnRepository columnRepository)
    {
        _boardRepository = boardRepository;
        _columnRepository = columnRepository;
    }

    public async Task<BoardDto> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var board = Board.Create(request.Name, request.Description);
        await _boardRepository.CreateAsync(board);

        var newColumn = BoardColumn.Create("New", 0, board.Id, isProtected: true);
        await _columnRepository.CreateAsync(newColumn);

        await _boardRepository.SaveChangesAsync();
        
        return new BoardDto
        {
            Id = board.Id,
            Name = board.Name,
            Description = board.Description
        };
    }
}
