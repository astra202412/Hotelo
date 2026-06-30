using Hotelo.Core.Entities.Reservation;

namespace Hotelo.Core.Interfaces.Repositories;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetAllWithDetailsAsync();
    Task<Reservation?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Reservation>> GetTodayArrivalsAsync();
    Task<IEnumerable<Reservation>> GetTodayDeparturesAsync();
    Task<IEnumerable<Reservation>> GetByDateRangeAsync(DateTime from, DateTime to);
    Task<IEnumerable<Reservation>> GetByGuestAsync(int guestId);
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeId = null);
}
