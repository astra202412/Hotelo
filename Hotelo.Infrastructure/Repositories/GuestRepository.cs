using Hotelo.Core.Entities.Reservation;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Repositories;

public class GuestRepository : GenericRepository<Guest>, IGuestRepository
{
    public GuestRepository(HoteloDbContext context) : base(context) { }

    public async Task<IEnumerable<Guest>> SearchAsync(string term)
        => await _context.Guests
            .Where(g => !g.IsDeleted && (
                g.FullName.Contains(term) ||
                (g.Passport != null && g.Passport.Contains(term)) ||
                (g.Phone != null && g.Phone.Contains(term)) ||
                (g.Email != null && g.Email.Contains(term))))
            .OrderBy(g => g.FullName)
            .ToListAsync();

    public async Task<IEnumerable<Guest>> GetWithReservationsAsync()
        => await _context.Guests
            .Include(g => g.Reservations)
            .Where(g => !g.IsDeleted)
            .ToListAsync();
}
