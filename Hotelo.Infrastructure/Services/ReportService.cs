using Hotelo.Common.Enums;
using Hotelo.Core.DTOs.Reports;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Core.Interfaces.Services;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Services;

public class ReportService : IReportService
{
    private readonly HoteloDbContext _context;
    private readonly IRoomRepository _roomRepo;
    private readonly IReservationRepository _resRepo;
    private readonly IInvoiceRepository _invoiceRepo;
    private readonly IHRRepository _hrRepo;

    public ReportService(HoteloDbContext context,
                         IRoomRepository roomRepo,
                         IReservationRepository resRepo,
                         IInvoiceRepository invoiceRepo,
                         IHRRepository hrRepo)
    {
        _context = context;
        _roomRepo = roomRepo;
        _resRepo = resRepo;
        _invoiceRepo = invoiceRepo;
        _hrRepo = hrRepo;
    }

    public async Task<DirectionReportDto> GetDirectionReportAsync(int year, int month)
    {
        var from = new DateTime(year, month, 1);
        var to = from.AddMonths(1);
        var prevFrom = from.AddMonths(-1);
        var prevTo = from;

        // ── Chambres ──────────────────────────────────────────────────────
        var roomCounts = await _roomRepo.GetStatusCountsAsync();
        var allRooms = (await _roomRepo.GetAllWithDetailsAsync()).ToList();
        var totalRooms = allRooms.Count;
        var occupied = roomCounts.GetValueOrDefault(RoomStatus.Occupee, 0);
        var maintenance = roomCounts.GetValueOrDefault(RoomStatus.Maintenance, 0);

        // ── Reservations ──────────────────────────────────────────────────
        var allRes = await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room).ThenInclude(r => r.RoomType)
            .IgnoreQueryFilters()
            .ToListAsync();

        var monthRes = allRes.Where(r => r.CheckIn >= from && r.CheckIn < to).ToList();
        var prevRes = allRes.Where(r => r.CheckIn >= prevFrom && r.CheckIn < prevTo).ToList();
        var checkedIn = monthRes.Count(r => r.Status == ReservationStatus.CheckedIn ||
                                              r.Status == ReservationStatus.CheckedOut);
        var cancelled = monthRes.Count(r => r.Status == ReservationStatus.Annulee);
        var avgStay = monthRes.Any()
            ? monthRes.Average(r => (r.CheckOut - r.CheckIn).TotalDays)
            : 0;

        // ── Finances ──────────────────────────────────────────────────────
        var allPayments = await _context.Payments.ToListAsync();
        var monthPay = allPayments.Where(p => p.PaidAt >= from && p.PaidAt < to).Sum(p => p.Amount);
        var prevPay = allPayments.Where(p => p.PaidAt >= prevFrom && p.PaidAt < prevTo).Sum(p => p.Amount);
        var yearPay = allPayments.Where(p => p.PaidAt.Year == year).Sum(p => p.Amount);

        var invoices = await _context.Invoices.ToListAsync();
        var monthInv = invoices.Where(i => i.CreatedAt >= from && i.CreatedAt < to).ToList();

        // ── KPIs Hôteliers ────────────────────────────────────────────────
        var occupancyRate = totalRooms > 0
            ? Math.Round((decimal)occupied / totalRooms * 100, 1) : 0;
        var prevOccupancy = totalRooms > 0
            ? Math.Round((decimal)prevRes.Count(r => r.Status == ReservationStatus.CheckedIn) / totalRooms * 100, 1) : 0;

        var adr = checkedIn > 0 ? Math.Round(monthPay / checkedIn, 0) : 0;
        var revpar = totalRooms > 0 ? Math.Round(monthPay / totalRooms, 0) : 0;

        // ── Revenus par mois (12 derniers mois) ───────────────────────────
        var monthlyRevenue = new List<MonthlyRevenueDto>();
        for (int i = 11; i >= 0; i--)
        {
            var mFrom = new DateTime(year, month, 1).AddMonths(-i);
            var mTo = mFrom.AddMonths(1);
            var mRev = allPayments.Where(p => p.PaidAt >= mFrom && p.PaidAt < mTo).Sum(p => p.Amount);
            var mStays = allRes.Count(r => r.CheckIn >= mFrom && r.CheckIn < mTo &&
                                           r.Status != ReservationStatus.Annulee);
            monthlyRevenue.Add(new MonthlyRevenueDto
            {
                Month = mFrom.ToString("MMM yy"),
                Revenue = mRev,
                Stays = mStays
            });
        }

        // ── Occupation par type de chambre ────────────────────────────────
        var occupancyByType = allRooms
            .GroupBy(r => r.RoomType?.Name ?? "Inconnu")
            .Select(g =>
            {
                var typeOcc = g.Count(r => r.Status == RoomStatus.Occupee);
                var typeRev = allPayments
                    .Join(invoices, p => p.InvoiceId, i => i.Id, (p, i) => new { p, i })
                    .Where(x => allRes.Any(r => r.Id == x.i.ReservationId &&
                                                g.Any(rm => rm.Id == r.RoomId) &&
                                                x.p.PaidAt >= from && x.p.PaidAt < to))
                    .Sum(x => x.p.Amount);
                return new OccupancyByTypeDto
                {
                    RoomType = g.Key,
                    Total = g.Count(),
                    Occupied = typeOcc,
                    OccupancyRate = g.Count() > 0 ? Math.Round((decimal)typeOcc / g.Count() * 100, 1) : 0,
                    Revenue = typeRev
                };
            }).OrderByDescending(x => x.OccupancyRate).ToList();

        // ── Top Clients ───────────────────────────────────────────────────
        var topGuests = allRes
            .Where(r => r.Guest != null)
            .GroupBy(r => new { r.GuestId, r.Guest!.FullName, r.Guest.VIPLevel })
            .Select(g => new TopGuestDto
            {
                GuestName = g.Key.FullName,
                Stays = g.Count(),
                TotalSpent = g.Sum(r => r.TotalAmount),
                VIPLevel = g.Key.VIPLevel switch { 1 => "Silver", 2 => "Gold", 3 => "Platinum", _ => "Standard" }
            })
            .OrderByDescending(x => x.TotalSpent)
            .Take(5)
            .ToList();

        // ── Revenus par mois detail ───────────────────────────────────────
        var revenueByMonth = monthlyRevenue.Select(m => new RevenueByModuleDto
        {
            Month = m.Month,
            Revenue = m.Revenue,
            Reservations = m.Stays,
            AvgRevPerStay = m.Stays > 0 ? Math.Round(m.Revenue / m.Stays, 0) : 0
        }).ToList();

        // ── RH ────────────────────────────────────────────────────────────
        var employees = await _context.Employees.Where(e => e.IsActive).ToListAsync();

        return new DirectionReportDto
        {
            OccupancyRate = occupancyRate,
            RevPAR = revpar,
            ADR = adr,
            TotalRevenue = monthPay,
            RevenuePerGuest = checkedIn > 0 ? Math.Round(monthPay / checkedIn, 0) : 0,
            OccupancyRatePrevMonth = prevOccupancy,
            RevenuePrevMonth = prevPay,
            OccupancyVariation = occupancyRate - prevOccupancy,
            RevenueVariation = prevPay > 0 ? Math.Round((monthPay - prevPay) / prevPay * 100, 1) : 0,
            TotalRooms = totalRooms,
            OccupiedRooms = occupied,
            AvailableRooms = roomCounts.GetValueOrDefault(RoomStatus.Libre, 0),
            MaintenanceRooms = maintenance,
            TotalReservations = monthRes.Count,
            CheckedInCount = checkedIn,
            CancelledCount = cancelled,
            CancellationRate = monthRes.Any() ? Math.Round((decimal)cancelled / monthRes.Count * 100, 1) : 0,
            AverageStayDays = Math.Round((decimal)avgStay, 1),
            RevenueMonth = monthPay,
            RevenueYear = yearPay,
            PaidInvoices = invoices.Count(i => i.IsPaid),
            UnpaidInvoices = invoices.Count(i => !i.IsPaid),
            TotalEmployees = employees.Count,
            AverageSalary = employees.Any() ? Math.Round(employees.Average(e => e.BaseSalary), 0) : 0,
            MonthlyRevenue = monthlyRevenue,
            OccupancyByType = occupancyByType,
            TopGuests = topGuests,
            RevenueByMonth = revenueByMonth
        };
    }
}
