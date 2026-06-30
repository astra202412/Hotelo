using Microsoft.AspNetCore.Identity;

namespace Hotelo.Core.Entities.Identity;

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}