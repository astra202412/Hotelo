using Hotelo.Core.Entities.Housekeeping;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Repositories;

public class HousekeepingRepository : GenericRepository<HousekeepingTask>, IHousekeepingRepository
{
    public HousekeepingRepository(HoteloDbContext context) : base(context) { }

    public async Task<IEnumerable<HousekeepingTask>> GetAllWithDetailsAsync()
        => await _context.HousekeepingTasks
            .OrderBy(t => t.Priority == "Urgent" ? 0 : t.Priority == "Haute" ? 1 : 2)
            .ThenBy(t => t.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<HousekeepingTask>> GetPendingAsync()
        => await _context.HousekeepingTasks
            .Where(t => t.Status != "Termine")
            .OrderBy(t => t.Priority == "Urgent" ? 0 : 1)
            .ToListAsync();

    public async Task<IEnumerable<HousekeepingTask>> GetByRoomAsync(int roomId)
        => await _context.HousekeepingTasks
            .Where(t => t.RoomId == roomId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<MaintenanceRequest>> GetAllMaintenanceAsync()
        => await _context.MaintenanceRequests
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<MaintenanceRequest>> GetOpenMaintenanceAsync()
        => await _context.MaintenanceRequests
            .Where(m => m.Status != "Ferme")
            .OrderBy(m => m.Priority == "Urgent" ? 0 : 1)
            .ToListAsync();

    public async Task<MaintenanceRequest?> GetMaintenanceByIdAsync(int id)
        => await _context.MaintenanceRequests.FindAsync(id);
}
