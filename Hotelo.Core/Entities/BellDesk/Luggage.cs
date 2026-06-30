using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.BellDesk;

public class Luggage : AuditableEntity
{
    public int ReservationId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public int BagsCount { get; set; } = 1;
    public string Type { get; set; } = "Arrivee"; // Arrivee, Depart, Stockage
    public string Status { get; set; } = "EnAttente";
    public string? Tag { get; set; }
    public string? Notes { get; set; }
    public DateTime? HandledAt { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}
