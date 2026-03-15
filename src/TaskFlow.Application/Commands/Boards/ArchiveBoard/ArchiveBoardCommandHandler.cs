using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Errors;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Boards.ArchiveBoard;

public class ArchiveBoardCommandHandler : IRequestHandler<ArchiveBoardCommand, Result>
{
    private readonly IBoardRepository _boardRepository;

    public ArchiveBoardCommandHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<Result> Handle(ArchiveBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.Id);
        if (board == null)
        {
            return Result.Failure(BoardErrors.NotFound(request.Id));
        }
        
        if (board.IsArchived)
        {
            return Result.Failure(BoardErrors.AlreadyArchived(request.Id));
        }

        board.Archive();

        await _boardRepository.UpdateAsync(board);
        await _boardRepository.SaveChangesAsync();

        return Result.Success();
    }
}
