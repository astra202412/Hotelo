using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.Housekeeping;

public class HousekeepingTask : BaseEntity
{
    public int RoomId { get; set; }
    public string? AssignedTo { get; set; }
    public string Status { get; set; } = "EnAttente";
    public string Priority { get; set; } = "Normal";
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}
