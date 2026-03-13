using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Services;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardColumnRepository _columnRepository;

    public BoardService(IBoardRepository boardRepository, IBoardColumnRepository columnRepository)
    {
        _boardRepository = boardRepository;
        _columnRepository = columnRepository;
    }

    public async Task<List<BoardDto>> GetAllAsync()
    {
        var boards = await _boardRepository.GetAllAsync();
        return boards.Select(ToDto).ToList();
    }

    public async Task<BoardDetailDto?> GetByIdAsync(Guid id)
    {
        var board = await _boardRepository.GetWithColumnsAndItemsAsync(id);
        return board == null ? null : ToDetailDto(board);
    }

    public async Task<BoardDto> CreateAsync(CreateBoardDto dto)
    {
        var board = Board.Create(dto.Name, dto.Description);
        await _boardRepository.CreateAsync(board);

        var newColumn = BoardColumn.Create("New", 0, board.Id, isProtected: true);
        await _columnRepository.CreateAsync(newColumn);

        await _boardRepository.SaveChangesAsync();
        return ToDto(board);
    }

    public async Task<BoardDto?> UpdateAsync(Guid id, UpdateBoardDto dto)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null)
            return null;

        if (dto.Name != null) board.Name = dto.Name;
        if (dto.Description != null) board.Description = dto.Description;
        board.UpdatedAt = DateTime.UtcNow;

        await _boardRepository.UpdateAsync(board);
        await _boardRepository.SaveChangesAsync();
        return ToDto(board);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var board = await _boardRepository.GetByIdAsync(id);
        if (board == null)
            return false;

        await _boardRepository.DeleteAsync(board);
        await _boardRepository.SaveChangesAsync();
        return true;
    }

    private static BoardDto ToDto(Board b) => new()
    {
        Id = b.Id,
        Name = b.Name,
        Description = b.Description
    };

    private static BoardDetailDto ToDetailDto(Board b) => new()
    {
        Id = b.Id,
        Name = b.Name,
        Description = b.Description,
        Columns = b.Columns
            .OrderBy(c => c.Order)
            .Select(c => new ColumnDetailDto
            {
                Id = c.Id,
                Name = c.Name,
                Order = c.Order,
                IsProtected = c.IsProtected,
                WorkItems = c.WorkItems
                    .OrderBy(w => w.Order)
                    .Select(w => new WorkItemDto
                    {
                        Id = w.Id,
                        Title = w.Title,
                        Description = w.Description,
                        Order = w.Order
                    }).ToList()
            }).ToList()
    };
}
