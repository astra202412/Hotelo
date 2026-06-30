using Hotelo.Common.Enums;
using Hotelo.Core.DTOs.Room;

namespace Hotelo.Core.Interfaces.Services;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllAsync();
    Task<IEnumerable<RoomDto>> GetByFloorAsync(int floorId);
    Task<RoomDto?> GetByIdAsync(int id);
    Task<RoomDto> CreateAsync(CreateRoomDto dto);
    Task<RoomDto> UpdateAsync(int id, UpdateRoomDto dto);
    Task DeleteAsync(int id);
    Task UpdateStatusAsync(int id, RoomStatus status);
    Task<FloorPlanDto> GetFloorPlanAsync();
    Task<FrontOfficeDashboardDto> GetDashboardAsync();
}
