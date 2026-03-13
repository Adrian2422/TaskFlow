using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Infrastructure.AppDbContext;

namespace TaskFlow.Infrastructure.Persistence;

public class BoardRepository : IBoardRepository
{
    private readonly ApplicationDbContext _context;

    public BoardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Board>> GetAllAsync()
    {
        return await _context.Boards.ToListAsync();
    }

    public async Task<Board?> GetByIdAsync(Guid id)
    {
        return await _context.Boards.FindAsync(id);
    }

    public async Task<Board?> GetWithColumnsAndItemsAsync(Guid id)
    {
        return await _context.Boards
            .Include(b => b.BacklogItems)
            .Include(b => b.Columns)
                .ThenInclude(c => c.WorkItems)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task CreateAsync(Board board)
    {
        await _context.Boards.AddAsync(board);
    }

    public async Task UpdateAsync(Board board)
    {
        _context.Boards.Update(board);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Board board)
    {
        _context.Boards.Remove(board);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
