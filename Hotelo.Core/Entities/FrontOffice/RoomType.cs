using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.FrontOffice;

public class RoomType : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public int MaxOccupancy { get; set; }
    public bool IsActive { get; set; } = true;
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}