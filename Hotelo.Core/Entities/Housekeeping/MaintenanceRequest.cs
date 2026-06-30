using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.Housekeeping;

public class MaintenanceRequest : BaseEntity
{
    public int RoomId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Normal";
    public string Status { get; set; } = "Ouvert";
    public string? AssignedTo { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? Resolution { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}
