namespace Hotelo.Core.DTOs.Reports;

public class DirectionReportDto
{
    // KPIs Hôteliers
    public decimal OccupancyRate { get; set; }
    public decimal RevPAR { get; set; }
    public decimal ADR { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal RevenuePerGuest { get; set; }

    // Comparatif
    public decimal OccupancyRatePrevMonth { get; set; }
    public decimal RevenuePrevMonth { get; set; }
    public decimal OccupancyVariation { get; set; }
    public decimal RevenueVariation { get; set; }

    // Chambres
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public int AvailableRooms { get; set; }
    public int MaintenanceRooms { get; set; }

    // Reservations
    public int TotalReservations { get; set; }
    public int CheckedInCount { get; set; }
    public int CancelledCount { get; set; }
    public decimal CancellationRate { get; set; }
    public decimal AverageStayDays { get; set; }

    // Finances
    public decimal RevenueMonth { get; set; }
    public decimal RevenueYear { get; set; }
    public int PaidInvoices { get; set; }
    public int UnpaidInvoices { get; set; }

    // RH
    public int TotalEmployees { get; set; }
    public decimal AverageSalary { get; set; }

    // Graphiques
    public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
    public List<OccupancyByTypeDto> OccupancyByType { get; set; } = new();
    public List<TopGuestDto> TopGuests { get; set; } = new();
    public List<RevenueByModuleDto> RevenueByMonth { get; set; } = new();
}

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int Stays { get; set; }
}

public class OccupancyByTypeDto
{
    public string RoomType { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Occupied { get; set; }
    public decimal OccupancyRate { get; set; }
    public decimal Revenue { get; set; }
}

public class TopGuestDto
{
    public string GuestName { get; set; } = string.Empty;
    public int Stays { get; set; }
    public decimal TotalSpent { get; set; }
    public string VIPLevel { get; set; } = string.Empty;
}

public class RevenueByModuleDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int Reservations { get; set; }
    public decimal AvgRevPerStay { get; set; }
}
