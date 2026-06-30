using Hotelo.Common.Enums;
using Hotelo.Core.Entities.FrontOffice;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(HoteloDbContext context) : base(context) { }

    public async Task<IEnumerable<Room>> GetAllWithDetailsAsync()
        => await _context.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Floor)
            .Where(r => r.IsActive)
            .OrderBy(r => r.Floor.Number)
            .ThenBy(r => r.RoomNumber)
            .ToListAsync();

    public async Task<IEnumerable<Room>> GetByFloorAsync(int floorId)
        => await _context.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Floor)
            .Where(r => r.FloorId == floorId && r.IsActive)
            .OrderBy(r => r.RoomNumber)
            .ToListAsync();

    public async Task<IEnumerable<Room>> GetByStatusAsync(RoomStatus status)
        => await _context.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Floor)
            .Where(r => r.Status == status && r.IsActive)
            .ToListAsync();

    public async Task<Room?> GetWithDetailsAsync(int id)
        => await _context.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Floor)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
        => await _context.Rooms
            .Include(r => r.RoomType)
            .Include(r => r.Floor)
            .Where(r => r.IsActive && r.Status == RoomStatus.Libre)
            .ToListAsync();

    public async Task<Dictionary<RoomStatus, int>> GetStatusCountsAsync()
        => await _context.Rooms
            .Where(r => r.IsActive)
            .GroupBy(r => r.Status)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
}
