using Hotelo.Core.Entities.Finances;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Repositories;

public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(HoteloDbContext context) : base(context) { }

    public async Task<IEnumerable<Invoice>> GetAllWithDetailsAsync()
        => await _context.Invoices
            .Include(i => i.Payments)
            .Include(i => i.Reservation).ThenInclude(r => r.Guest)
            .Include(i => i.Reservation).ThenInclude(r => r.Room).ThenInclude(r => r.RoomType)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

    public async Task<Invoice?> GetWithDetailsAsync(int id)
        => await _context.Invoices
            .Include(i => i.Payments)
            .Include(i => i.Reservation).ThenInclude(r => r.Guest)
            .Include(i => i.Reservation).ThenInclude(r => r.Room).ThenInclude(r => r.RoomType)
            .FirstOrDefaultAsync(i => i.Id == id);

    public async Task<Invoice?> GetByReservationAsync(int reservationId)
        => await _context.Invoices
            .Include(i => i.Payments)
            .Include(i => i.Reservation).ThenInclude(r => r.Guest)
            .Include(i => i.Reservation).ThenInclude(r => r.Room).ThenInclude(r => r.RoomType)
            .FirstOrDefaultAsync(i => i.ReservationId == reservationId);

    public async Task<IEnumerable<Invoice>> GetUnpaidAsync()
        => await _context.Invoices
            .Include(i => i.Reservation).ThenInclude(r => r.Guest)
            .Include(i => i.Reservation).ThenInclude(r => r.Room)
            .Where(i => !i.IsPaid)
            .OrderBy(i => i.CreatedAt)
            .ToListAsync();

    public async Task<decimal> GetRevenueTodayAsync()
        => await _context.Payments
            .Where(p => p.PaidAt.Date == DateTime.Today)
            .SumAsync(p => p.Amount);

    public async Task<decimal> GetRevenueMonthAsync()
        => await _context.Payments
            .Where(p => p.PaidAt.Year == DateTime.Today.Year &&
                        p.PaidAt.Month == DateTime.Today.Month)
            .SumAsync(p => p.Amount);

    public async Task<List<(DateTime Date, decimal Revenue)>> GetRevenueLast7DaysAsync()
    {
        var from = DateTime.Today.AddDays(-6);
        var data = await _context.Payments
            .Where(p => p.PaidAt.Date >= from)
            .GroupBy(p => p.PaidAt.Date)
            .Select(g => new { Date = g.Key, Revenue = g.Sum(p => p.Amount) })
            .ToListAsync();
        return data.Select(d => (d.Date, d.Revenue)).ToList();
    }
}
