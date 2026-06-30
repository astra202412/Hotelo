using Hotelo.Core.DTOs.Technique;
using Hotelo.Core.Entities.Housekeeping;
using Hotelo.Core.Interfaces.Services;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Services;

public class TechniqueService : ITechniqueService
{
    private readonly HoteloDbContext _context;
    public TechniqueService(HoteloDbContext context) => _context = context;

    private static string GetCategoryColor(string c) => c switch
    {
        "Electricite" => "#FFC107",
        "Plomberie" => "#2196F3",
        "Climatisation" => "#00BCD4",
        _ => "#9E9E9E"
    };
    private static string GetPriorityColor(string p) => p switch
    {
        "Urgent" => "#F44336",
        "Haute" => "#FF9800",
        _ => "#2196F3"
    };
    private static string GetStatusColor(string s) => s switch
    {
        "Ouvert" => "#F44336",
        "EnCours" => "#FF9800",
        "Ferme" => "#4CAF50",
        _ => "#9E9E9E"
    };

    private static TechInterventionDto ToDto(TechIntervention t) => new()
    {
        Id = t.Id,
        RoomId = t.RoomId,
        Title = t.Title,
        Description = t.Description,
        Category = t.Category,
        CategoryColor = GetCategoryColor(t.Category),
        Priority = t.Priority,
        PriorityColor = GetPriorityColor(t.Priority),
        Status = t.Status,
        StatusColor = GetStatusColor(t.Status),
        AssignedTo = t.AssignedTo,
        ScheduledAt = t.ScheduledAt,
        ClosedAt = t.ClosedAt,
        Resolution = t.Resolution,
        Cost = t.Cost,
        CreatedAt = t.CreatedAt
    };

    public async Task<TechniqueDashboardDto> GetDashboardAsync()
    {
        var items = await _context.TechInterventions.OrderByDescending(t => t.CreatedAt).ToListAsync();
        var today = DateTime.Today;
        return new TechniqueDashboardDto
        {
            TotalOpen = items.Count(t => t.Status != "Ferme"),
            InProgress = items.Count(t => t.Status == "EnCours"),
            ClosedThisMonth = items.Count(t => t.ClosedAt?.Month == today.Month && t.ClosedAt?.Year == today.Year),
            Urgent = items.Count(t => t.Priority == "Urgent" && t.Status != "Ferme"),
            TotalCostMonth = items.Where(t => t.ClosedAt?.Month == today.Month).Sum(t => t.Cost),
            Interventions = items.Select(ToDto).ToList()
        };
    }

    public async Task<TechInterventionDto> CreateAsync(CreateTechDto dto)
    {
        var item = new TechIntervention
        {
            RoomId = dto.RoomId,
            Title = dto.Title,
            Description = dto.Description,
            Category = dto.Category,
            Priority = dto.Priority,
            Status = "Ouvert",
            AssignedTo = dto.AssignedTo,
            ScheduledAt = dto.ScheduledAt,
            CreatedAt = DateTime.UtcNow
        };
        _context.TechInterventions.Add(item);
        await _context.SaveChangesAsync();
        return ToDto(item);
    }

    public async Task StartAsync(int id)
    {
        var t = await _context.TechInterventions.FindAsync(id)
            ?? throw new KeyNotFoundException();
        t.Status = "EnCours";
        await _context.SaveChangesAsync();
    }

    public async Task CloseAsync(int id, string resolution, decimal cost)
    {
        var t = await _context.TechInterventions.FindAsync(id)
            ?? throw new KeyNotFoundException();
        t.Status = "Ferme";
        t.Resolution = resolution;
        t.Cost = cost;
        t.ClosedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}
