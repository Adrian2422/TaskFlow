using MediatR;
using TaskFlow.Application.Common;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Common;

namespace TaskFlow.Application.Commands.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.AuthenticateAsync(request.Email, request.Password);

        if (!result.Succeeded)
        {
            return Result.Failure<AuthResponseDto>(new Error("Auth.Unauthorized", result.Error ?? "Invalid credentials"));
        }

        return Result.Success(new AuthResponseDto()
        {
            Token = result.Token!
        });
    }
}
