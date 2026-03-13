using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces;

public interface IBoardRepository
{
    Task<List<Board>> GetAllAsync();
    Task<Board?> GetByIdAsync(Guid id);
    Task<Board?> GetWithColumnsAndItemsAsync(Guid id);
    Task CreateAsync(Board board);
    Task UpdateAsync(Board board);
    Task DeleteAsync(Board board);
    Task SaveChangesAsync();
}
