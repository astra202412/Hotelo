using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.HR;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}