using Hotelo.Core.Entities.Reservation;

namespace Hotelo.Core.Interfaces.Repositories;

public interface IGuestRepository : IGenericRepository<Guest>
{
    Task<IEnumerable<Guest>> SearchAsync(string term);
    Task<IEnumerable<Guest>> GetWithReservationsAsync();
}
