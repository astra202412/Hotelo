using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.Commercial;

public class Contract : AuditableEntity
{
    public int CompanyId { get; set; }
    public string Reference { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal RoomRate { get; set; }
    public decimal DiscountRate { get; set; }
    public int RoomsQuota { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
    public virtual Company Company { get; set; } = null!;
}
