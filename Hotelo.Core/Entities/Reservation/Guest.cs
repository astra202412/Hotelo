using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.Reservation;

public class Guest : AuditableEntity
{
    public string FullName { get; set; } = string.Empty;
    public string? Passport { get; set; }
    public string? Nationality { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int VIPLevel { get; set; } = 0;
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}