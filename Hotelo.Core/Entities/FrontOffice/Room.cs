using Hotelo.Common.Enums;
using Hotelo.Core.Entities.Common;

namespace Hotelo.Core.Entities.FrontOffice;

public class Room : AuditableEntity
{
    public string RoomNumber { get; set; } = string.Empty;
    public int RoomTypeId { get; set; }
    public int FloorId { get; set; }
    public RoomStatus Status { get; set; } = RoomStatus.Libre;
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
    public virtual RoomType RoomType { get; set; } = null!;
    public virtual Floor Floor { get; set; } = null!;
}