using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/boards")]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _service;

    public BoardsController(IBoardService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<BoardDto>>> GetAll()
    {
        var boards = await _service.GetAllAsync();
        return Ok(boards);
    }

    [HttpGet("{boardId:guid}")]
    public async Task<ActionResult<BoardDetailDto>> GetById(Guid boardId)
    {
        var board = await _service.GetByIdAsync(boardId);
        if (board == null)
            return NotFound();

        return Ok(board);
    }

    [HttpPost]
    public async Task<ActionResult<BoardDto>> Create([FromBody] CreateBoardDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { boardId = result.Id }, result);
    }

    [HttpPut("{boardId:guid}")]
    public async Task<ActionResult<BoardDto>> Update(Guid boardId, [FromBody] UpdateBoardDto dto)
    {
        var result = await _service.UpdateAsync(boardId, dto);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{boardId:guid}")]
    public async Task<ActionResult> Delete(Guid boardId)
    {
        var deleted = await _service.DeleteAsync(boardId);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
