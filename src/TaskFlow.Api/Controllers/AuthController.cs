using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskFlow.Application.Commands.Auth.Login;
using TaskFlow.Application.Commands.Auth.Register;
using TaskFlow.Application.DTOs.Auth;

namespace TaskFlow.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Login",
        Description = "Login and get token",
        OperationId = "Login",
        Tags = ["Auth"]
    )]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));
        return HandleResult(result);
    }

    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "Register",
        Description = "Register",
        OperationId = "Register",
        Tags = ["Auth"]
    )]
    public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequest request)
    {
        var result = await _mediator.Send(new RegisterCommand(request.Email, request.Password));
        return HandleResult(result);
    }
}

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Email, string FullName, string Password);
