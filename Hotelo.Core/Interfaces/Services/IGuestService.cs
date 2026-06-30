using Hotelo.Core.DTOs.Guest;

namespace Hotelo.Core.Interfaces.Services;

public interface IGuestService
{
    Task<IEnumerable<GuestDto>> GetAllAsync();
    Task<GuestDto?> GetByIdAsync(int id);
    Task<GuestDto> CreateAsync(CreateGuestDto dto);
    Task<GuestDto> UpdateAsync(int id, UpdateGuestDto dto);
    Task DeleteAsync(int id);
    Task<IEnumerable<GuestDto>> SearchAsync(string term);
}
