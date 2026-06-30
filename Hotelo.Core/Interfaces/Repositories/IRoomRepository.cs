using Hotelo.Common.Enums;
using Hotelo.Core.Entities.FrontOffice;

namespace Hotelo.Core.Interfaces.Repositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task<IEnumerable<Room>> GetAllWithDetailsAsync();
    Task<IEnumerable<Room>> GetByFloorAsync(int floorId);
    Task<IEnumerable<Room>> GetByStatusAsync(RoomStatus status);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
    Task<Room?> GetWithDetailsAsync(int id);
    Task<Dictionary<RoomStatus, int>> GetStatusCountsAsync();
}
