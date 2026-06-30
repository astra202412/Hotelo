using Hotelo.Core.DTOs.Housekeeping;

namespace Hotelo.Core.Interfaces.Services;

public interface IHousekeepingService
{
    Task<HousekeepingDashboardDto> GetDashboardAsync();
    Task<IEnumerable<HousekeepingTaskDto>> GetAllTasksAsync();
    Task<HousekeepingTaskDto> CreateTaskAsync(CreateTaskDto dto);
    Task AssignTaskAsync(int taskId, string userId);
    Task StartTaskAsync(int taskId);
    Task CompleteTaskAsync(int taskId);
    Task<IEnumerable<MaintenanceRequestDto>> GetAllMaintenanceAsync();
    Task<MaintenanceRequestDto> CreateMaintenanceAsync(CreateMaintenanceDto dto);
    Task CloseMaintenanceAsync(int id, string resolution);
}
