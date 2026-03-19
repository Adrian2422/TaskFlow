using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public ICollection<BoardMember> BoardMemberships { get; set; } = new List<BoardMember>();
}
