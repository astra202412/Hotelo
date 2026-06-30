using Hotelo.Core.Entities.Housekeeping;

namespace Hotelo.Core.Interfaces.Repositories;

public interface IHousekeepingRepository : IGenericRepository<HousekeepingTask>
{
    Task<IEnumerable<HousekeepingTask>> GetAllWithDetailsAsync();
    Task<IEnumerable<HousekeepingTask>> GetPendingAsync();
    Task<IEnumerable<HousekeepingTask>> GetByRoomAsync(int roomId);
    Task<IEnumerable<MaintenanceRequest>> GetAllMaintenanceAsync();
    Task<IEnumerable<MaintenanceRequest>> GetOpenMaintenanceAsync();
    Task<MaintenanceRequest?> GetMaintenanceByIdAsync(int id);
}
