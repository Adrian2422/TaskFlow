using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

public interface IIdentityService
{
    Task<(bool Succeeded, string? Token, string? Error)> AuthenticateAsync(string email, string password);
    Task<(bool Succeeded, Guid? UserId, IEnumerable<string> Errors)> RegisterAsync(string email, string password);
    Task<bool> AuthorizeAsync(Guid userId, string policyName);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
}
