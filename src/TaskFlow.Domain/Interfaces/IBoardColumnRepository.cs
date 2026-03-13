using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces;

public interface IBoardColumnRepository
{
    Task<BoardColumn?> GetByIdAsync(Guid id);
    Task<List<BoardColumn>> GetByBoardIdAsync(Guid boardId);
    Task<double> GetMaxOrderInBoardAsync(Guid boardId);
    Task CreateAsync(BoardColumn column);
    Task UpdateAsync(BoardColumn column);
    Task DeleteAsync(BoardColumn column);
    Task SaveChangesAsync();
}
