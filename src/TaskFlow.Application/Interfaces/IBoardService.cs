using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;

public interface IBoardService
{
    Task<List<BoardDto>> GetAllAsync();
    Task<BoardDetailDto?> GetByIdAsync(Guid id);
    Task<BoardDto> CreateAsync(CreateBoardDto dto);
    Task<BoardDto?> UpdateAsync(Guid id, UpdateBoardDto dto);
    Task<bool> DeleteAsync(Guid id);
}
