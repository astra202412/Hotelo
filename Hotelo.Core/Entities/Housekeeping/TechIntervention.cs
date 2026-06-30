using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.Housekeeping;

public class TechIntervention : BaseEntity
{
    public int RoomId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "General"; // Electricite, Plomberie, Climatisation, General
    public string Priority { get; set; } = "Normal";
    public string Status { get; set; } = "Ouvert";
    public string? AssignedTo { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? Resolution { get; set; }
    public decimal Cost { get; set; } = 0;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}
