using Hotelo.Core.DTOs.BellDesk;

namespace Hotelo.Core.Interfaces.Services;

public interface IBellDeskService
{
    Task<BellDeskDashboardDto> GetDashboardAsync();
    Task<LuggageDto> CreateLuggageAsync(CreateLuggageDto dto);
    Task HandleLuggageAsync(int id);
    Task<GuestServiceDto> CreateServiceAsync(CreateGuestServiceDto dto);
    Task CompleteServiceAsync(int id);
}
