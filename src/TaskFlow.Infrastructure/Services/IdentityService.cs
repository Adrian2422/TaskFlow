using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<(bool Succeeded, string? Token, string? Error)> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (false, null, "Invalid credentials");
        }

        var result = await _userManager.CheckPasswordAsync(user, password);
        if (!result)
        {
            return (false, null, "Invalid credentials");
        }

        var token = GenerateJwtToken(user);
        return (true, token, null);
    }

    public async Task<(bool Succeeded, Guid? UserId, IEnumerable<string> Errors)> RegisterAsync(string email, string password)
    {
        var user = new User
        {
            UserName = email,
            Email = email,
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return (false, null, result.Errors.Select(e => e.Description));
        }

        return (true, user.Id, Enumerable.Empty<string>());
    }

    public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
    {
        // Simplification: In a real app we'd use IAuthorizationService
        return true; 
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return Enumerable.Empty<string>();
        return await _userManager.GetRolesAsync(user);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim("fullName", user.FullName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "vI8A_u-9hS7y-Y2X-n5_jR3m-Q8L_p6Z")); // Default for development
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"] ?? "7"));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"] ?? "TaskFlow",
            _configuration["Jwt:Audience"] ?? "TaskFlow",
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
