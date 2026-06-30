using Hotelo.Core.DTOs.Commercial;

namespace Hotelo.Core.Interfaces.Services;

public interface ICommercialService
{
    Task<CommercialDashboardDto> GetDashboardAsync();
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
    Task<CompanyDto?> GetCompanyByIdAsync(int id);
    Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto);
    Task DeleteCompanyAsync(int id);
    Task<IEnumerable<ContractDto>> GetAllContractsAsync();
    Task<ContractDto> CreateContractAsync(CreateContractDto dto);
}
