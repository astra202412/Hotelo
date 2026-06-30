using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.FrontOffice;

public class Floor : BaseEntity
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}