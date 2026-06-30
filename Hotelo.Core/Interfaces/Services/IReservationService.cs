using Hotelo.Core.DTOs.Reservation;

namespace Hotelo.Core.Interfaces.Services;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetAllAsync();
    Task<IEnumerable<ReservationDto>> GetTodayArrivalsAsync();
    Task<IEnumerable<ReservationDto>> GetTodayDeparturesAsync();
    Task<IEnumerable<ReservationDto>> GetByDateRangeAsync(DateTime from, DateTime to);
    Task<IEnumerable<ReservationDto>> GetByGuestAsync(int guestId);
    Task<ReservationDto?> GetByIdAsync(int id);
    Task<ReservationDto> CreateAsync(CreateReservationDto dto);
    Task<ReservationDto> UpdateAsync(int id, UpdateReservationDto dto);
    Task CheckInAsync(int id);
    Task CheckOutAsync(int id);
    Task CancelAsync(int id, string reason);
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeId = null);
}
