using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Boards.DeleteBoard;

public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand, Result>
{
    private readonly IBoardRepository _boardRepository;

    public DeleteBoardCommandHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.Id);
        if (board == null)
        {
            return Result.Failure(BoardErrors.NotFound(request.Id));
        }

        if (!board.IsArchived)
        {
            return Result.Failure(BoardErrors.DeleteNotArchived);
        }

        await _boardRepository.DeleteAsync(board);
        await _boardRepository.SaveChangesAsync();

        return Result.Success();
    }
}
