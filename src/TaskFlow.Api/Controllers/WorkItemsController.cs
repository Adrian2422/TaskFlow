using TaskFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/work-items")]
public class WorkItemsController : ControllerBase
{
    private readonly IWorkItemService _service;

    public WorkItemsController(IWorkItemService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedResult<WorkItemDto>>> GetWorkItems([FromQuery] PaginationQuery query)
    {
        var result = await _service.GetPagedAsync(query);
        return Ok(result);
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<WorkItemDto>>> GetAllWorkItems()
    {
        var workItems = await _service.GetAllAsync();
        return Ok(workItems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkItemDto>> GetById(Guid id)
    {
        var item = await _service.GetByIdAsync(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<WorkItemDto>> Create(CreateWorkItemDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WorkItemDto>> Update(Guid id, UpdateWorkItemDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/move")]
    public async Task<ActionResult> Move(Guid id, [FromBody] MoveWorkItemDto dto)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null)
            return NotFound();

        await _service.MoveAsync(id, dto);

        return NoContent();
    }
}