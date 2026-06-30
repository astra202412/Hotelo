namespace Hotelo.Core.DTOs.BellDesk;

public class LuggageDto
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public int BagsCount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string TypeColor { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = string.Empty;
    public string? Tag { get; set; }
    public string? Notes { get; set; }
    public DateTime? HandledAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateLuggageDto
{
    public int ReservationId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public int BagsCount { get; set; } = 1;
    public string Type { get; set; } = "Arrivee";
    public string? Notes { get; set; }
}

public class GuestServiceDto
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
}

public class CreateGuestServiceDto
{
    public int ReservationId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? ScheduledAt { get; set; }
    public string? Notes { get; set; }
}

public class BellDeskDashboardDto
{
    public int TotalLuggageToday { get; set; }
    public int PendingLuggage { get; set; }
    public int PendingServices { get; set; }
    public int CompletedToday { get; set; }
    public List<LuggageDto> Luggages { get; set; } = new();
    public List<GuestServiceDto> Services { get; set; } = new();
}
