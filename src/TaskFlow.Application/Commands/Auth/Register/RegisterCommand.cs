using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs.Auth;

namespace TaskFlow.Application.Commands.Auth.Register;

public record RegisterCommand(string Email, string Password) : IRequest<Result<RegisterResponseDto>>;
