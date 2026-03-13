using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Services;

public class BoardColumnService : IBoardColumnService
{
    private readonly IBoardColumnRepository _columnRepository;
    private readonly IBoardRepository _boardRepository;

    public BoardColumnService(IBoardColumnRepository columnRepository, IBoardRepository boardRepository)
    {
        _columnRepository = columnRepository;
        _boardRepository = boardRepository;
    }

    public async Task<List<ColumnDto>> GetByBoardIdAsync(Guid boardId)
    {
        var columns = await _columnRepository.GetByBoardIdAsync(boardId);
        return columns.OrderBy(c => c.Order).Select(ToDto).ToList();
    }

    public async Task<ColumnDto> CreateAsync(Guid boardId, CreateColumnDto dto)
    {
        var maxOrder = await _columnRepository.GetMaxOrderInBoardAsync(boardId);
        var column = BoardColumn.Create(dto.Name, maxOrder + 1000, boardId);
        await _columnRepository.CreateAsync(column);
        await _columnRepository.SaveChangesAsync();
        return ToDto(column);
    }

    public async Task<ColumnDto?> UpdateAsync(Guid boardId, Guid columnId, UpdateColumnDto dto)
    {
        var column = await _columnRepository.GetByIdAsync(columnId);
        if (column == null || column.BoardId != boardId)
            return null;

        if (dto.Name != null) column.Name = dto.Name;
        if (dto.Order.HasValue) column.Order = dto.Order.Value;
        column.UpdatedAt = DateTime.UtcNow;

        await _columnRepository.UpdateAsync(column);
        await _columnRepository.SaveChangesAsync();
        return ToDto(column);
    }

    public async Task<bool> DeleteAsync(Guid boardId, Guid columnId)
    {
        var column = await _columnRepository.GetByIdAsync(columnId);
        if (column == null || column.BoardId != boardId)
            return false;

        if (column.IsProtected)
            throw new InvalidOperationException("Cannot delete a protected column.");

        await _columnRepository.DeleteAsync(column);
        await _columnRepository.SaveChangesAsync();
        return true;
    }

    public async Task MoveAsync(Guid boardId, Guid columnId, MoveColumnDto dto)
    {
        var column = await _columnRepository.GetByIdAsync(columnId);
        if (column == null || column.BoardId != boardId) return;

        double newOrder;
        if (dto.PrevPosition.HasValue && dto.NextPosition.HasValue)
            newOrder = (dto.PrevPosition.Value + dto.NextPosition.Value) / 2.0;
        else if (dto.PrevPosition.HasValue)
            newOrder = dto.PrevPosition.Value + 1000;
        else if (dto.NextPosition.HasValue)
            newOrder = dto.NextPosition.Value / 2.0;
        else
            newOrder = 1000;

        column.Order = newOrder;
        await _columnRepository.UpdateAsync(column);
        await _columnRepository.SaveChangesAsync();
    }

    private static ColumnDto ToDto(BoardColumn c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Order = c.Order,
        IsProtected = c.IsProtected
    };
}
