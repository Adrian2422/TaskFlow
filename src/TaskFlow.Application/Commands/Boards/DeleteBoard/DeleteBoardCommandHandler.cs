using MediatR;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Commands.Boards.DeleteBoard;

public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand, bool>
{
    private readonly IBoardRepository _boardRepository;

    public DeleteBoardCommandHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<bool> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(request.Id);
        if (board == null)
            return false;

        await _boardRepository.DeleteAsync(board);
        await _boardRepository.SaveChangesAsync();
        return true;
    }
}
