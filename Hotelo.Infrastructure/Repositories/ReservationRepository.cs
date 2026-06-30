using Hotelo.Common.Enums;
using Hotelo.Core.Entities.Reservation;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(HoteloDbContext context) : base(context) { }

    public async Task<IEnumerable<Reservation>> GetAllWithDetailsAsync()
        => await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room).ThenInclude(r => r.RoomType)
            .Include(r => r.Package)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    public async Task<Reservation?> GetWithDetailsAsync(int id)
        => await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room).ThenInclude(r => r.RoomType)
            .Include(r => r.Package)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Reservation>> GetTodayArrivalsAsync()
    {
        var today = DateTime.Today;
        return await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Where(r => r.CheckIn.Date == today &&
                       (r.Status == ReservationStatus.Confirmee ||
                        r.Status == ReservationStatus.EnAttente))
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetTodayDeparturesAsync()
    {
        var today = DateTime.Today;
        return await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Where(r => r.CheckOut.Date == today &&
                        r.Status == ReservationStatus.CheckedIn)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime from, DateTime to)
        => await _context.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
            .Where(r => r.CheckIn.Date >= from.Date && r.CheckIn.Date <= to.Date)
            .OrderBy(r => r.CheckIn)
            .ToListAsync();

    public async Task<IEnumerable<Reservation>> GetByGuestAsync(int guestId)
        => await _context.Reservations
            .Include(r => r.Room).ThenInclude(r => r.RoomType)
            .Include(r => r.Package)
            .Where(r => r.GuestId == guestId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeId = null)
    {
        var query = _context.Reservations
            .Where(r => r.RoomId == roomId &&
                        r.Status != ReservationStatus.Annulee &&
                        r.Status != ReservationStatus.NoShow &&
                        r.CheckIn < checkOut &&
                        r.CheckOut > checkIn);

        if (excludeId.HasValue)
            query = query.Where(r => r.Id != excludeId.Value);

        return !await query.AnyAsync();
    }
}
