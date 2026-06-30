using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.Finances;

public class Invoice : AuditableEntity
{
    public int ReservationId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal TotalHT { get; set; }
    public decimal TVARate { get; set; } = 0.19m;
    public decimal TVAAmount { get; set; }
    public decimal TotalTTC { get; set; }
    public decimal Discount { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime? PaidAt { get; set; }
    public string? Notes { get; set; }

    public virtual Hotelo.Core.Entities.Reservation.Reservation Reservation { get; set; } = null!;
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
