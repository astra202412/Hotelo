using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.Commercial;

public class Company : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "Entreprise"; // Entreprise, Agence, Ambassade
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? ContactPerson { get; set; }
    public decimal DiscountRate { get; set; } = 0; // % remise contractuelle
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
