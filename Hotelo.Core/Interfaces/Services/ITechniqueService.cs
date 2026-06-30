using Hotelo.Core.DTOs.Technique;

namespace Hotelo.Core.Interfaces.Services;

public interface ITechniqueService
{
    Task<TechniqueDashboardDto> GetDashboardAsync();
    Task<TechInterventionDto> CreateAsync(CreateTechDto dto);
    Task StartAsync(int id);
    Task CloseAsync(int id, string resolution, decimal cost);
}
