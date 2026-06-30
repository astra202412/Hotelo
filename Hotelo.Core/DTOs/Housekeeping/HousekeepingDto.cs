namespace Hotelo.Core.DTOs.Housekeeping;

public class HousekeepingTaskDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public string RoomStatus { get; set; } = string.Empty;
    public string? AssignedTo { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string PriorityColor { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTaskDto
{
    public int RoomId { get; set; }
    public string? AssignedTo { get; set; }
    public string Priority { get; set; } = "Normal";
    public DateTime? DueDate { get; set; }
    public string? Notes { get; set; }
}

public class MaintenanceRequestDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string PriorityColor { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = string.Empty;
    public string? AssignedTo { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? Resolution { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateMaintenanceDto
{
    public int RoomId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Normal";
    public string? AssignedTo { get; set; }
}

public class HousekeepingDashboardDto
{
    public int TotalTasks { get; set; }
    public int PendingTasks { get; set; }
    public int InProgressTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int OpenMaintenance { get; set; }
    public int UrgentTasks { get; set; }
    public List<HousekeepingTaskDto> TasksList { get; set; } = new();
    public List<MaintenanceRequestDto> MaintenanceList { get; set; } = new();
}
