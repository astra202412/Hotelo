using Hotelo.Core.DTOs.Dashboard;

namespace Hotelo.Core.Interfaces.Services;

public interface IDashboardService
{
    Task<GlobalDashboardDto> GetGlobalDashboardAsync();
}
