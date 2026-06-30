using Hotelo.Core.DTOs.Guest;
using Hotelo.Core.Entities.Reservation;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Core.Interfaces.Services;

namespace Hotelo.Infrastructure.Services;

public class GuestService : IGuestService
{
    private readonly IGuestRepository _guestRepo;

    public GuestService(IGuestRepository guestRepo)
    {
        _guestRepo = guestRepo;
    }

    private static string GetVIPLabel(int level) => level switch
    {
        1 => "Silver",
        2 => "Gold",
        3 => "Platinum",
        _ => "Standard"
    };

    private static GuestDto ToDto(Guest g) => new()
    {
        Id = g.Id,
        FullName = g.FullName,
        Passport = g.Passport,
        Nationality = g.Nationality,
        Email = g.Email,
        Phone = g.Phone,
        VIPLevel = g.VIPLevel,
        VIPLabel = GetVIPLabel(g.VIPLevel),
        Address = g.Address,
        Notes = g.Notes,
        TotalStays = g.Reservations?.Count ?? 0
    };

    public async Task<IEnumerable<GuestDto>> GetAllAsync()
        => (await _guestRepo.GetWithReservationsAsync()).Select(ToDto);

    public async Task<GuestDto?> GetByIdAsync(int id)
    {
        var g = await _guestRepo.GetByIdAsync(id);
        return g == null ? null : ToDto(g);
    }

    public async Task<IEnumerable<GuestDto>> SearchAsync(string term)
        => (await _guestRepo.SearchAsync(term)).Select(ToDto);

    public async Task<GuestDto> CreateAsync(CreateGuestDto dto)
    {
        var guest = new Guest
        {
            FullName = dto.FullName,
            Passport = dto.Passport,
            Nationality = dto.Nationality,
            Email = dto.Email,
            Phone = dto.Phone,
            VIPLevel = dto.VIPLevel,
            Address = dto.Address,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };
        await _guestRepo.AddAsync(guest);
        return ToDto(guest);
    }

    public async Task<GuestDto> UpdateAsync(int id, UpdateGuestDto dto)
    {
        var guest = await _guestRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Client {id} introuvable");
        guest.FullName = dto.FullName;
        guest.Passport = dto.Passport;
        guest.Nationality = dto.Nationality;
        guest.Email = dto.Email;
        guest.Phone = dto.Phone;
        guest.VIPLevel = dto.VIPLevel;
        guest.Address = dto.Address;
        guest.Notes = dto.Notes;
        await _guestRepo.UpdateAsync(guest);
        return ToDto(guest);
    }

    public async Task DeleteAsync(int id)
    {
        var guest = await _guestRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Client {id} introuvable");
        guest.IsDeleted = true;
        await _guestRepo.UpdateAsync(guest);
    }
}
