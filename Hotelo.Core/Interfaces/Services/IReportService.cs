using Hotelo.Core.DTOs.Reports;

namespace Hotelo.Core.Interfaces.Services;

public interface IReportService
{
    Task<DirectionReportDto> GetDirectionReportAsync(int year, int month);
}
