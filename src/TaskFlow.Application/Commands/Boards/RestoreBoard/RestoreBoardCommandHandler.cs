using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Boards.RestoreBoard;

public class RestoreBoardCommandHandler : IRequestHandler<RestoreBoardCommand, Result>
{
    private readonly IBoardRepository _boardRepository;

    public RestoreBoardCommandHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result> Handle(RestoreBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.Id);
        if (board == null)
        {
            return Result.Failure(BoardErrors.NotFound(request.Id));
        }

        if (!board.IsArchived)
        {
            return Result.Failure(BoardErrors.RestoreNotArchived);
        }

        board.Restore();

        await _boardRepository.UpdateAsync(board);
        await _boardRepository.SaveChangesAsync();

        return Result.Success();
    }
}
