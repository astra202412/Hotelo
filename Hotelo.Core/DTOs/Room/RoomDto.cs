using Hotelo.Common.Enums;

namespace Hotelo.Core.DTOs.Room;

public class RoomDto
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public int RoomTypeId { get; set; }
    public string Floor { get; set; } = string.Empty;
    public int FloorId { get; set; }
    public int FloorNumber { get; set; }
    public RoomStatus Status { get; set; }
    public string StatusLabel { get; set; } = string.Empty;
    public string StatusColor { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public int MaxOccupancy { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
}

public class CreateRoomDto
{
    public string RoomNumber { get; set; } = string.Empty;
    public int RoomTypeId { get; set; }
    public int FloorId { get; set; }
    public string? Notes { get; set; }
}

public class UpdateRoomDto : CreateRoomDto
{
    public bool IsActive { get; set; } = true;
}

public class UpdateRoomStatusDto
{
    public RoomStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class FloorPlanDto
{
    public List<FloorDto> Floors { get; set; } = new();
    public Dictionary<string, int> StatusCounts { get; set; } = new();
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public decimal OccupancyRate { get; set; }
}

public class FloorDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<RoomDto> Rooms { get; set; } = new();
}

public class FrontOfficeDashboardDto
{
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public int FreeRooms { get; set; }
    public int MaintenanceRooms { get; set; }
    public int CleaningRooms { get; set; }
    public int ReservedRooms { get; set; }
    public decimal OccupancyRate { get; set; }
    public int TodayArrivals { get; set; }
    public int TodayDepartures { get; set; }
}
