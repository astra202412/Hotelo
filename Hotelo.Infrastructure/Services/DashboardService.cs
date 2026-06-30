using Hotelo.Common.Enums;
using Hotelo.Core.DTOs.Dashboard;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Core.Interfaces.Services;

namespace Hotelo.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly IRoomRepository _roomRepo;
    private readonly IReservationRepository _resRepo;
    private readonly IInvoiceRepository _invoiceRepo;

    public DashboardService(IRoomRepository roomRepo,
                            IReservationRepository resRepo,
                            IInvoiceRepository invoiceRepo)
    {
        _roomRepo = roomRepo;
        _resRepo = resRepo;
        _invoiceRepo = invoiceRepo;
    }

    public async Task<GlobalDashboardDto> GetGlobalDashboardAsync()
    {
        // --- Chambres ---
        var roomCounts = await _roomRepo.GetStatusCountsAsync();
        var allRooms = (await _roomRepo.GetAllWithDetailsAsync()).ToList();
        var totalRooms = allRooms.Count;
        var occupied = roomCounts.GetValueOrDefault(RoomStatus.Occupee, 0);
        var free = roomCounts.GetValueOrDefault(RoomStatus.Libre, 0);
        var maintenance = roomCounts.GetValueOrDefault(RoomStatus.Maintenance, 0);
        var cleaning = roomCounts.GetValueOrDefault(RoomStatus.ANettoyage, 0)
                        + roomCounts.GetValueOrDefault(RoomStatus.EnNettoyage, 0);
        var reserved = roomCounts.GetValueOrDefault(RoomStatus.Reservee, 0);

        // --- Reservations ---
        var arrivals = (await _resRepo.GetTodayArrivalsAsync()).ToList();
        var departures = (await _resRepo.GetTodayDeparturesAsync()).ToList();
        var allRes = (await _resRepo.GetAllWithDetailsAsync()).ToList();
        var activeRes = allRes.Count(r => r.Status == ReservationStatus.CheckedIn);
        var guests = allRes.Select(r => r.GuestId).Distinct().Count();

        // --- Finances ---
        var revenueToday = await _invoiceRepo.GetRevenueTodayAsync();
        var revenueMonth = await _invoiceRepo.GetRevenueMonthAsync();
        var allInvoices = (await _invoiceRepo.GetAllWithDetailsAsync()).ToList();
        var paidInvoices = allInvoices.Count(i => i.IsPaid);
        var unpaidInvoices = allInvoices.Count(i => !i.IsPaid);
        var pendingAmount = allInvoices.Where(i => !i.IsPaid).Sum(i => i.TotalTTC);
        var last7 = await _invoiceRepo.GetRevenueLast7DaysAsync();

        // --- Graphique Revenus 7 jours ---
        var revenueChart = Enumerable.Range(0, 7)
            .Select(d => DateTime.Today.AddDays(-6 + d))
            .Select(date => new RevenueChartDto
            {
                Day = date.ToString("dd/MM"),
                Revenue = last7.FirstOrDefault(x => x.Date == date).Revenue
            }).ToList();

        // --- Graphique Occupation par Etage ---
        var occupancyByFloor = allRooms
            .GroupBy(r => new { r.FloorId, r.Floor?.Name, r.Floor?.Number })
            .OrderBy(g => g.Key.Number)
            .Select(g =>
            {
                var total = g.Count();
                var occ = g.Count(r => r.Status == RoomStatus.Occupee);
                return new OccupancyChartDto
                {
                    Floor = g.Key.Name ?? "",
                    Total = total,
                    Occupied = occ,
                    Rate = total > 0 ? Math.Round((decimal)occ / total * 100, 1) : 0
                };
            }).ToList();

        // --- Graphique Statuts Chambres ---
        var statusChart = roomCounts
            .Where(k => k.Value > 0)
            .Select(k => new StatusChartDto
            {
                Label = GetStatusLabel(k.Key),
                Count = k.Value,
                Color = GetStatusColor(k.Key)
            }).ToList();

        // --- Mouvements du jour ---
        var arrivalsList = arrivals.Select(r => new TodayMovementDto
        {
            GuestName = r.Guest?.FullName ?? "",
            RoomNumber = r.Room?.RoomNumber ?? "",
            RoomType = r.Room?.RoomType?.Name ?? "",
            Time = r.CheckIn.ToString("HH:mm"),
            Status = "Arrivee",
            StatusColor = "#2196F3"
        }).ToList();

        var departuresList = departures.Select(r => new TodayMovementDto
        {
            GuestName = r.Guest?.FullName ?? "",
            RoomNumber = r.Room?.RoomNumber ?? "",
            RoomType = r.Room?.RoomType?.Name ?? "",
            Time = r.CheckOut.ToString("HH:mm"),
            Status = "Depart",
            StatusColor = "#F44336"
        }).ToList();

        return new GlobalDashboardDto
        {
            TotalRooms = totalRooms,
            OccupiedRooms = occupied,
            FreeRooms = free,
            MaintenanceRooms = maintenance,
            CleaningRooms = cleaning,
            ReservedRooms = reserved,
            OccupancyRate = totalRooms > 0 ? Math.Round((decimal)occupied / totalRooms * 100, 1) : 0,
            TodayArrivals = arrivals.Count,
            TodayDepartures = departures.Count,
            ActiveReservations = activeRes,
            TotalGuests = guests,
            RevenueToday = revenueToday,
            RevenueMonth = revenueMonth,
            PaidInvoices = paidInvoices,
            UnpaidInvoices = unpaidInvoices,
            PendingAmount = pendingAmount,
            RevenueLast7Days = revenueChart,
            OccupancyByFloor = occupancyByFloor,
            RoomStatusChart = statusChart,
            TodayArrivalsList = arrivalsList,
            TodayDeparturesList = departuresList
        };
    }

    private static string GetStatusLabel(RoomStatus s) => s switch
    {
        RoomStatus.Libre => "Libre",
        RoomStatus.Occupee => "Occupee",
        RoomStatus.ANettoyage => "A Nettoyer",
        RoomStatus.EnNettoyage => "En Nettoyage",
        RoomStatus.Maintenance => "Maintenance",
        RoomStatus.Bloquee => "Bloquee",
        RoomStatus.Reservee => "Reservee",
        _ => "Inconnu"
    };

    private static string GetStatusColor(RoomStatus s) => s switch
    {
        RoomStatus.Libre => "#4CAF50",
        RoomStatus.Occupee => "#F44336",
        RoomStatus.ANettoyage => "#FFC107",
        RoomStatus.EnNettoyage => "#FF9800",
        RoomStatus.Maintenance => "#9E9E9E",
        RoomStatus.Bloquee => "#1A237E",
        RoomStatus.Reservee => "#2196F3",
        _ => "#000000"
    };
}
