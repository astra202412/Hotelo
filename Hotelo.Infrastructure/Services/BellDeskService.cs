using Hotelo.Core.DTOs.BellDesk;
using Hotelo.Core.Entities.BellDesk;
using Hotelo.Core.Interfaces.Services;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Services;

public class BellDeskService : IBellDeskService
{
    private readonly HoteloDbContext _context;
    public BellDeskService(HoteloDbContext context) => _context = context;

    private static string GetTypeColor(string t) => t switch
    {
        "Arrivee" => "#2196F3",
        "Depart" => "#F44336",
        "Stockage" => "#FF9800",
        _ => "#9E9E9E"
    };

    private static string GetStatusColor(string s) => s switch
    {
        "EnAttente" => "#FFC107",
        "EnCours" => "#2196F3",
        "Livre" => "#4CAF50",
        "Termine" => "#4CAF50",
        _ => "#9E9E9E"
    };

    private static LuggageDto LuggageToDto(Luggage l) => new()
    {
        Id = l.Id,
        ReservationId = l.ReservationId,
        GuestName = l.GuestName,
        RoomNumber = l.RoomNumber,
        BagsCount = l.BagsCount,
        Type = l.Type,
        TypeColor = GetTypeColor(l.Type),
        Status = l.Status,
        StatusColor = GetStatusColor(l.Status),
        Tag = l.Tag,
        Notes = l.Notes,
        HandledAt = l.HandledAt,
        CreatedAt = l.CreatedAt
    };

    private static GuestServiceDto SvcToDto(ConciergeService s) => new()
    {
        Id = s.Id,
        ReservationId = s.ReservationId,
        GuestName = s.GuestName,
        RoomNumber = s.RoomNumber,
        ServiceType = s.ServiceType,
        Description = s.Description,
        Status = s.Status,
        StatusColor = GetStatusColor(s.Status),
        RequestedAt = s.RequestedAt,
        ScheduledAt = s.ScheduledAt,
        CompletedAt = s.CompletedAt,
        Notes = s.Notes
    };

    public async Task<BellDeskDashboardDto> GetDashboardAsync()
    {
        var today = DateTime.Today;
        var luggages = await _context.Luggages.OrderByDescending(l => l.CreatedAt).ToListAsync();
        var services = await _context.GuestServices.OrderByDescending(s => s.RequestedAt).ToListAsync();

        return new BellDeskDashboardDto
        {
            TotalLuggageToday = luggages.Count(l => l.CreatedAt?.Date == today),
            PendingLuggage = luggages.Count(l => l.Status == "EnAttente"),
            PendingServices = services.Count(s => s.Status == "EnAttente"),
            CompletedToday = luggages.Count(l => l.HandledAt?.Date == today && l.Status == "Livre")
                              + services.Count(s => s.CompletedAt?.Date == today),
            Luggages = luggages.Select(LuggageToDto).ToList(),
            Services = services.Select(SvcToDto).ToList()
        };
    }

    public async Task<LuggageDto> CreateLuggageAsync(CreateLuggageDto dto)
    {
        var luggage = new Luggage
        {
            ReservationId = dto.ReservationId,
            GuestName = dto.GuestName,
            RoomNumber = dto.RoomNumber,
            BagsCount = dto.BagsCount,
            Type = dto.Type,
            Status = "EnAttente",
            Tag = $"TAG{DateTime.Now:yyyyMMddHHmmss}",
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };
        _context.Luggages.Add(luggage);
        await _context.SaveChangesAsync();
        return LuggageToDto(luggage);
    }

    public async Task HandleLuggageAsync(int id)
    {
        var l = await _context.Luggages.FindAsync(id)
            ?? throw new KeyNotFoundException($"Bagage {id} introuvable");
        l.Status = "Livre";
        l.HandledAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<GuestServiceDto> CreateServiceAsync(CreateGuestServiceDto dto)
    {
        var svc = new ConciergeService
        {
            ReservationId = dto.ReservationId,
            GuestName = dto.GuestName,
            RoomNumber = dto.RoomNumber,
            ServiceType = dto.ServiceType,
            Description = dto.Description,
            Status = "EnAttente",
            RequestedAt = DateTime.UtcNow,
            ScheduledAt = dto.ScheduledAt,
            Notes = dto.Notes
        };
        _context.GuestServices.Add(svc);
        await _context.SaveChangesAsync();
        return SvcToDto(svc);
    }

    public async Task CompleteServiceAsync(int id)
    {
        var s = await _context.GuestServices.FindAsync(id)
            ?? throw new KeyNotFoundException($"Service {id} introuvable");
        s.Status = "Termine";
        s.CompletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}
