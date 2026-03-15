using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Common;
using TaskFlow.Domain.Common;

namespace TaskFlow.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return MapError(result.Error);
    }

    protected ActionResult<T> HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return MapError(result.Error);
    }

    private ActionResult MapError(Error error)
    {
        if (error.Code.EndsWith(".NotFound"))
        {
            return NotFound(error);
        }

        return BadRequest(error);
    }
}
