using Hotelo.Common.Enums;
using Hotelo.Core.DTOs.Room;
using Hotelo.Core.Entities.FrontOffice;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Core.Interfaces.Services;

namespace Hotelo.Infrastructure.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepo;

    public RoomService(IRoomRepository roomRepo)
    {
        _roomRepo = roomRepo;
    }

    private static string GetStatusLabel(RoomStatus s) => s switch
    {
        RoomStatus.Libre => "Libre",
        RoomStatus.Occupee => "Occupee",
        RoomStatus.ANettoyage => "A Nettoyer",
        RoomStatus.EnNettoyage => "En Nettoyage",
        RoomStatus.Maintenance => "Maintenance",
        RoomStatus.Bloquee => "Bloquee",
        RoomStatus.Reservee => "Reservee",
        _ => "Inconnu"
    };

    private static string GetStatusColor(RoomStatus s) => s switch
    {
        RoomStatus.Libre => "#4CAF50",
        RoomStatus.Occupee => "#F44336",
        RoomStatus.ANettoyage => "#FFC107",
        RoomStatus.EnNettoyage => "#FF9800",
        RoomStatus.Maintenance => "#9E9E9E",
        RoomStatus.Bloquee => "#1A237E",
        RoomStatus.Reservee => "#2196F3",
        _ => "#000000"
    };

    private static RoomDto ToDto(Room r) => new()
    {
        Id = r.Id,
        RoomNumber = r.RoomNumber,
        RoomType = r.RoomType?.Name ?? "",
        RoomTypeId = r.RoomTypeId,
        Floor = r.Floor?.Name ?? "",
        FloorId = r.FloorId,
        FloorNumber = r.Floor?.Number ?? 0,
        Status = r.Status,
        StatusLabel = GetStatusLabel(r.Status),
        StatusColor = GetStatusColor(r.Status),
        BasePrice = r.RoomType?.BasePrice ?? 0,
        MaxOccupancy = r.RoomType?.MaxOccupancy ?? 0,
        IsActive = r.IsActive,
        Notes = r.Notes
    };

    public async Task<IEnumerable<RoomDto>> GetAllAsync()
        => (await _roomRepo.GetAllWithDetailsAsync()).Select(ToDto);

    public async Task<IEnumerable<RoomDto>> GetByFloorAsync(int floorId)
        => (await _roomRepo.GetByFloorAsync(floorId)).Select(ToDto);

    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        var room = await _roomRepo.GetWithDetailsAsync(id);
        return room == null ? null : ToDto(room);
    }

    public async Task<RoomDto> CreateAsync(CreateRoomDto dto)
    {
        var room = new Room
        {
            RoomNumber = dto.RoomNumber,
            RoomTypeId = dto.RoomTypeId,
            FloorId = dto.FloorId,
            Status = RoomStatus.Libre,
            IsActive = true,
            Notes = dto.Notes
        };
        await _roomRepo.AddAsync(room);
        return ToDto(await _roomRepo.GetWithDetailsAsync(room.Id) ?? room);
    }

    public async Task<RoomDto> UpdateAsync(int id, UpdateRoomDto dto)
    {
        var room = await _roomRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Chambre {id} introuvable");
        room.RoomNumber = dto.RoomNumber;
        room.RoomTypeId = dto.RoomTypeId;
        room.FloorId = dto.FloorId;
        room.IsActive = dto.IsActive;
        room.Notes = dto.Notes;
        await _roomRepo.UpdateAsync(room);
        return ToDto(await _roomRepo.GetWithDetailsAsync(id) ?? room);
    }

    public async Task DeleteAsync(int id)
    {
        var room = await _roomRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Chambre {id} introuvable");
        room.IsActive = false;
        await _roomRepo.UpdateAsync(room);
    }

    public async Task UpdateStatusAsync(int id, RoomStatus status)
    {
        var room = await _roomRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Chambre {id} introuvable");
        room.Status = status;
        await _roomRepo.UpdateAsync(room);
    }

    public async Task<FloorPlanDto> GetFloorPlanAsync()
    {
        var rooms = (await _roomRepo.GetAllWithDetailsAsync()).ToList();
        var counts = await _roomRepo.GetStatusCountsAsync();

        var floors = rooms
            .GroupBy(r => new { r.FloorId, r.Floor?.Number, r.Floor?.Name })
            .OrderBy(g => g.Key.Number)
            .Select(g => new FloorDto
            {
                Id = g.Key.FloorId,
                Number = g.Key.Number ?? 0,
                Name = g.Key.Name ?? "",
                Rooms = g.Select(ToDto).OrderBy(r => r.RoomNumber).ToList()
            }).ToList();

        var occupied = counts.GetValueOrDefault(RoomStatus.Occupee, 0);
        var total = rooms.Count;

        return new FloorPlanDto
        {
            Floors = floors,
            TotalRooms = total,
            OccupiedRooms = occupied,
            OccupancyRate = total > 0 ? Math.Round((decimal)occupied / total * 100, 1) : 0,
            StatusCounts = counts.ToDictionary(k => GetStatusLabel(k.Key), v => v.Value)
        };
    }

    public async Task<FrontOfficeDashboardDto> GetDashboardAsync()
    {
        var counts = await _roomRepo.GetStatusCountsAsync();
        var total = counts.Values.Sum();
        var occ = counts.GetValueOrDefault(RoomStatus.Occupee, 0);

        return new FrontOfficeDashboardDto
        {
            TotalRooms = total,
            OccupiedRooms = occ,
            FreeRooms = counts.GetValueOrDefault(RoomStatus.Libre, 0),
            MaintenanceRooms = counts.GetValueOrDefault(RoomStatus.Maintenance, 0),
            CleaningRooms = counts.GetValueOrDefault(RoomStatus.ANettoyage, 0)
                             + counts.GetValueOrDefault(RoomStatus.EnNettoyage, 0),
            ReservedRooms = counts.GetValueOrDefault(RoomStatus.Reservee, 0),
            OccupancyRate = total > 0 ? Math.Round((decimal)occ / total * 100, 1) : 0,
            TodayArrivals = 0,
            TodayDepartures = 0
        };
    }
}
