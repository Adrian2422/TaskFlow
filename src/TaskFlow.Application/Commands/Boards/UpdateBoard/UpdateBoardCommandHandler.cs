using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Boards.UpdateBoard;

public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, Result<BoardDto>>
{
    private readonly IBoardRepository _boardRepository;

    public UpdateBoardCommandHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result<BoardDto>> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.Id);
        if (board == null)
        {
            return Result.Failure<BoardDto>(BoardErrors.NotFound(request.Id));
        }

        if (request.Name != null) board.Name = request.Name;
        if (request.Description != null) board.Description = request.Description;

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
