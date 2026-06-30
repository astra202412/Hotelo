namespace Hotelo.Core.DTOs.Dashboard;

public class GlobalDashboardDto
{
    // KPIs Chambres
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public int FreeRooms { get; set; }
    public int MaintenanceRooms { get; set; }
    public int CleaningRooms { get; set; }
    public int ReservedRooms { get; set; }
    public decimal OccupancyRate { get; set; }

    // KPIs Reservations
    public int TodayArrivals { get; set; }
    public int TodayDepartures { get; set; }
    public int ActiveReservations { get; set; }
    public int TotalGuests { get; set; }

    // KPIs Finances
    public decimal RevenueToday { get; set; }
    public decimal RevenueMonth { get; set; }
    public int PaidInvoices { get; set; }
    public int UnpaidInvoices { get; set; }
    public decimal PendingAmount { get; set; }

    // Graphiques
    public List<RevenueChartDto> RevenueLast7Days { get; set; } = new();
    public List<OccupancyChartDto> OccupancyByFloor { get; set; } = new();
    public List<StatusChartDto> RoomStatusChart { get; set; } = new();

    // Arrivees et Departs du jour
    public List<TodayMovementDto> TodayArrivalsList { get; set; } = new();
    public List<TodayMovementDto> TodayDeparturesList { get; set; } = new();
}

public class RevenueChartDto
{
    public string Day { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

public class OccupancyChartDto
{
    public string Floor { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Occupied { get; set; }
    public decimal Rate { get; set; }
}

public class StatusChartDto
{
    public string Label { get; set; } = string.Empty;
    public int Count { get; set; }
    public string Color { get; set; } = string.Empty;
}

public class TodayMovementDto
{
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = string.Empty;
}
