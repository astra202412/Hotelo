using Hotelo.Common.Enums;
using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.Finances;

public class Payment : BaseEntity
{
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public string? Reference { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
    public virtual Invoice Invoice { get; set; } = null!;
}