using Hotelo.Common.Enums;
using Hotelo.Core.Entities.Common;
using Hotelo.Core.Entities.FrontOffice;

namespace Hotelo.Core.Entities.Reservation;

public class Reservation : AuditableEntity
{
    public int GuestId { get; set; }
    public int RoomId { get; set; }
    public int? PackageId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Adults { get; set; } = 1;
    public int Children { get; set; } = 0;
    public ReservationStatus Status { get; set; } = ReservationStatus.EnAttente;
    public decimal RoomRate { get; set; }
    public decimal PackagePrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? SpecialRequests { get; set; }
    public string? ConfirmationCode { get; set; }
    public virtual Guest Guest { get; set; } = null!;
    public virtual Room Room { get; set; } = null!;
    public virtual Package? Package { get; set; }
}