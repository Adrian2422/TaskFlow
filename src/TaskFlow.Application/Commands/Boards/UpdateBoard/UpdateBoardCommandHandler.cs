using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Boards.UpdateBoard;

public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, BoardDto?>
{
    private readonly IBoardRepository _boardRepository;

    public UpdateBoardCommandHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<BoardDto?> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.Id);
        if (board == null)
            return null;

        if (request.Name != null) board.Name = request.Name;
        if (request.Description != null) board.Description = request.Description;
        board.UpdatedAt = DateTime.UtcNow;

        await _boardRepository.UpdateAsync(board);
        await _boardRepository.SaveChangesAsync();

        return new BoardDto
        {
            Id = board.Id,
            Name = board.Name,
            Description = board.Description
        };
    }
}
