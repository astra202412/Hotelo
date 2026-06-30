using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.BellDesk;

public class ConciergeService : BaseEntity
{
    public int ReservationId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "EnAttente";
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ScheduledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
}
