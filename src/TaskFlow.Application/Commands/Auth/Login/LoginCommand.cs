using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs.Auth;

namespace TaskFlow.Application.Commands.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;
