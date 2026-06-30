using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.Reservation;

public class Package : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal AdditionalPrice { get; set; }
    public bool IncludesBreakfast { get; set; }
    public bool IncludesParking { get; set; }
    public bool IncludesSpa { get; set; }
    public bool IncludesAirportTransfer { get; set; }
    public int? MinNights { get; set; }
    public bool IsActive { get; set; } = true;
}