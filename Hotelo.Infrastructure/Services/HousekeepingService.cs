using Hotelo.Common.Enums;
using Hotelo.Core.DTOs.Housekeeping;
using Hotelo.Core.Entities.Housekeeping;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Core.Interfaces.Services;
using Hotelo.Infrastructure.Data;

namespace Hotelo.Infrastructure.Services;

public class HousekeepingService : IHousekeepingService
{
    private readonly IHousekeepingRepository _repo;
    private readonly IRoomRepository _roomRepo;
    private readonly HoteloDbContext _context;

    public HousekeepingService(IHousekeepingRepository repo,
                               IRoomRepository roomRepo,
                               HoteloDbContext context)
    {
        _repo = repo;
        _roomRepo = roomRepo;
        _context = context;
    }

    private static string GetPriorityColor(string p) => p switch
    {
        "Urgent" => "#F44336",
        "Haute" => "#FF9800",
        "Normal" => "#2196F3",
        _ => "#9E9E9E"
    };

    private static string GetStatusColor(string s) => s switch
    {
        "EnAttente" => "#FFC107",
        "EnCours" => "#2196F3",
        "Termine" => "#4CAF50",
        "Ouvert" => "#F44336",
        "EnTraitement" => "#FF9800",
        "Ferme" => "#9E9E9E",
        _ => "#9E9E9E"
    };

    private async Task<HousekeepingTaskDto> TaskToDto(HousekeepingTask t)
    {
        var room = await _roomRepo.GetWithDetailsAsync(t.RoomId);
        return new HousekeepingTaskDto
        {
            Id = t.Id,
            RoomId = t.RoomId,
            RoomNumber = room?.RoomNumber ?? "",
            Floor = room?.Floor?.Name ?? "",
            RoomType = room?.RoomType?.Name ?? "",
            RoomStatus = room?.Status.ToString() ?? "",
            AssignedTo = t.AssignedTo,
            Status = t.Status,
            Priority = t.Priority,
            PriorityColor = GetPriorityColor(t.Priority),
            DueDate = t.DueDate,
            CompletedAt = t.CompletedAt,
            Notes = t.Notes,
            CreatedAt = t.CreatedAt ?? DateTime.UtcNow
        };
    }

    private async Task<MaintenanceRequestDto> MaintenanceToDto(MaintenanceRequest m)
    {
        var room = await _roomRepo.GetWithDetailsAsync(m.RoomId);
        return new MaintenanceRequestDto
        {
            Id = m.Id,
            RoomId = m.RoomId,
            RoomNumber = room?.RoomNumber ?? "",
            Floor = room?.Floor?.Name ?? "",
            Description = m.Description,
            Priority = m.Priority,
            PriorityColor = GetPriorityColor(m.Priority),
            Status = m.Status,
            StatusColor = GetStatusColor(m.Status),
            AssignedTo = m.AssignedTo,
            ClosedAt = m.ClosedAt,
            Resolution = m.Resolution,
            CreatedAt = m.CreatedAt ?? DateTime.UtcNow
        };
    }

    public async Task<IEnumerable<HousekeepingTaskDto>> GetAllTasksAsync()
    {
        var tasks = await _repo.GetAllWithDetailsAsync();
        var result = new List<HousekeepingTaskDto>();
        foreach (var t in tasks) result.Add(await TaskToDto(t));
        return result;
    }

    public async Task<IEnumerable<MaintenanceRequestDto>> GetAllMaintenanceAsync()
    {
        var items = await _repo.GetAllMaintenanceAsync();
        var result = new List<MaintenanceRequestDto>();
        foreach (var m in items) result.Add(await MaintenanceToDto(m));
        return result;
    }

    public async Task<HousekeepingDashboardDto> GetDashboardAsync()
    {
        var tasks = (await _repo.GetAllWithDetailsAsync()).ToList();
        var maintenance = (await _repo.GetAllMaintenanceAsync()).ToList();

        var taskDtos = new List<HousekeepingTaskDto>();
        foreach (var t in tasks) taskDtos.Add(await TaskToDto(t));

        var maintDtos = new List<MaintenanceRequestDto>();
        foreach (var m in maintenance) maintDtos.Add(await MaintenanceToDto(m));

        return new HousekeepingDashboardDto
        {
            TotalTasks = tasks.Count,
            PendingTasks = tasks.Count(t => t.Status == "EnAttente"),
            InProgressTasks = tasks.Count(t => t.Status == "EnCours"),
            CompletedTasks = tasks.Count(t => t.Status == "Termine"),
            OpenMaintenance = maintenance.Count(m => m.Status != "Ferme"),
            UrgentTasks = tasks.Count(t => t.Priority == "Urgent") +
                              maintenance.Count(m => m.Priority == "Urgent" && m.Status != "Ferme"),
            TasksList = taskDtos,
            MaintenanceList = maintDtos
        };
    }

    public async Task<HousekeepingTaskDto> CreateTaskAsync(CreateTaskDto dto)
    {
        var task = new HousekeepingTask
        {
            RoomId = dto.RoomId,
            AssignedTo = dto.AssignedTo,
            Status = "EnAttente",
            Priority = dto.Priority,
            DueDate = dto.DueDate ?? DateTime.Today,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };
        await _repo.AddAsync(task);

        // Mettre la chambre en ANettoyage si Libre
        var room = await _roomRepo.GetByIdAsync(dto.RoomId);
        if (room != null && room.Status == RoomStatus.Libre)
        {
            room.Status = RoomStatus.ANettoyage;
            await _roomRepo.UpdateAsync(room);
        }
        return await TaskToDto(task);
    }

    public async Task AssignTaskAsync(int taskId, string userId)
    {
        var task = await _repo.GetByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Tache {taskId} introuvable");
        task.AssignedTo = userId;
        task.Status = "EnAttente";
        await _repo.UpdateAsync(task);
    }

    public async Task StartTaskAsync(int taskId)
    {
        var task = await _repo.GetByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Tache {taskId} introuvable");
        task.Status = "EnCours";
        await _repo.UpdateAsync(task);

        var room = await _roomRepo.GetByIdAsync(task.RoomId);
        if (room != null)
        {
            room.Status = RoomStatus.EnNettoyage;
            await _roomRepo.UpdateAsync(room);
        }
    }

    public async Task CompleteTaskAsync(int taskId)
    {
        var task = await _repo.GetByIdAsync(taskId)
            ?? throw new KeyNotFoundException($"Tache {taskId} introuvable");
        task.Status = "Termine";
        task.CompletedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(task);

        var room = await _roomRepo.GetByIdAsync(task.RoomId);
        if (room != null)
        {
            room.Status = RoomStatus.Libre;
            await _roomRepo.UpdateAsync(room);
        }
    }

    public async Task<MaintenanceRequestDto> CreateMaintenanceAsync(CreateMaintenanceDto dto)
    {
        var req = new MaintenanceRequest
        {
            RoomId = dto.RoomId,
            Description = dto.Description,
            Priority = dto.Priority,
            Status = "Ouvert",
            AssignedTo = dto.AssignedTo,
            CreatedAt = DateTime.UtcNow
        };
        _context.MaintenanceRequests.Add(req);
        await _context.SaveChangesAsync();

        var room = await _roomRepo.GetByIdAsync(dto.RoomId);
        if (room != null && dto.Priority == "Urgent")
        {
            room.Status = RoomStatus.Maintenance;
            await _roomRepo.UpdateAsync(room);
        }
        return await MaintenanceToDto(req);
    }

    public async Task CloseMaintenanceAsync(int id, string resolution)
    {
        var req = await _repo.GetMaintenanceByIdAsync(id)
            ?? throw new KeyNotFoundException($"Demande {id} introuvable");
        req.Status = "Ferme";
        req.Resolution = resolution;
        req.ClosedAt = DateTime.UtcNow;
        _context.MaintenanceRequests.Update(req);
        await _context.SaveChangesAsync();

        var room = await _roomRepo.GetByIdAsync(req.RoomId);
        if (room != null && room.Status == RoomStatus.Maintenance)
        {
            room.Status = RoomStatus.Libre;
            await _roomRepo.UpdateAsync(room);
        }
    }
}
