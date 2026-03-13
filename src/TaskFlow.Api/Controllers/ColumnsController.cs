using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/boards/{boardId:guid}/columns")]
public class ColumnsController : ControllerBase
{
    private readonly IBoardColumnService _service;

    public ColumnsController(IBoardColumnService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ColumnDto>>> GetAll(Guid boardId)
    {
        var columns = await _service.GetByBoardIdAsync(boardId);
        return Ok(columns);
    }

    [HttpPost]
    public async Task<ActionResult<ColumnDto>> Create(Guid boardId, [FromBody] CreateColumnDto dto)
    {
        var result = await _service.CreateAsync(boardId, dto);
        return Created(string.Empty, result);
    }

    [HttpPut("{columnId:guid}")]
    public async Task<ActionResult<ColumnDto>> Update(Guid boardId, Guid columnId, [FromBody] UpdateColumnDto dto)
    {
        var result = await _service.UpdateAsync(boardId, columnId, dto);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{columnId:guid}")]
    public async Task<ActionResult> Delete(Guid boardId, Guid columnId)
    {
        try
        {
            var deleted = await _service.DeleteAsync(boardId, columnId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("{columnId:guid}/move")]
    public async Task<ActionResult> Move(Guid boardId, Guid columnId, [FromBody] MoveColumnDto dto)
    {
        await _service.MoveAsync(boardId, columnId, dto);
        return NoContent();
    }
}
