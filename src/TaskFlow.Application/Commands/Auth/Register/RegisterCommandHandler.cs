using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Common;

namespace TaskFlow.Application.Commands.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponseDto>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<RegisterResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.RegisterAsync(request.Email, request.Password);

        if (!result.Succeeded)
        {
            var errorMessage = result.Errors != null ? string.Join(", ", result.Errors) : "Registration failed";
            return Result.Failure<RegisterResponseDto>(new Error("Auth.RegistrationFailed", errorMessage));
        }

        return Result.Success(new RegisterResponseDto()
        {
            UserId = result.UserId!.Value
        });
    }
}
