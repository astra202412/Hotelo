namespace Hotelo.Core.DTOs.Technique;

public class TechInterventionDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string PriorityColor { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = string.Empty;
    public string? AssignedTo { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public string? Resolution { get; set; }
    public decimal Cost { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CreateTechDto
{
    public int RoomId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public string Priority { get; set; } = "Normal";
    public string? AssignedTo { get; set; }
    public DateTime? ScheduledAt { get; set; }
}

public class TechniqueDashboardDto
{
    public int TotalOpen { get; set; }
    public int InProgress { get; set; }
    public int ClosedThisMonth { get; set; }
    public int Urgent { get; set; }
    public decimal TotalCostMonth { get; set; }
    public List<TechInterventionDto> Interventions { get; set; } = new();
}
