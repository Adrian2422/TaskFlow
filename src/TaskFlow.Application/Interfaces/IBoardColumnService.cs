using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;

public interface IBoardColumnService
{
    Task<List<ColumnDto>> GetByBoardIdAsync(Guid boardId);
    Task<ColumnDto> CreateAsync(Guid boardId, CreateColumnDto dto);
    Task<ColumnDto?> UpdateAsync(Guid boardId, Guid columnId, UpdateColumnDto dto);
    Task<bool> DeleteAsync(Guid boardId, Guid columnId);
    Task MoveAsync(Guid boardId, Guid columnId, MoveColumnDto dto);
}
