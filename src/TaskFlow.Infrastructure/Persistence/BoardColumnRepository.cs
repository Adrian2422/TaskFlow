using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Infrastructure.AppDbContext;

namespace TaskFlow.Infrastructure.Persistence;

public class BoardColumnRepository : IBoardColumnRepository
{
    private readonly ApplicationDbContext _context;

    public BoardColumnRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BoardColumn?> GetByIdAsync(Guid id)
    {
        return await _context.Columns.FindAsync(id);
    }

    public async Task<List<BoardColumn>> GetByBoardIdAsync(Guid boardId)
    {
        return await _context.Columns
            .Where(c => c.BoardId == boardId)
            .ToListAsync();
    }

    public async Task<double> GetMaxOrderInBoardAsync(Guid boardId)
    {
        return await _context.Columns
            .Where(c => c.BoardId == boardId)
            .MaxAsync(c => (double?)c.Order) ?? 0;
    }

    public async Task CreateAsync(BoardColumn column)
    {
        await _context.Columns.AddAsync(column);
    }

    public async Task UpdateAsync(BoardColumn column)
    {
        _context.Columns.Update(column);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(BoardColumn column)
    {
        _context.Columns.Remove(column);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
