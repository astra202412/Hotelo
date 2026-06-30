using Hotelo.Core.DTOs.Package;

namespace Hotelo.Core.Interfaces.Services;

public interface IPackageService
{
    Task<IEnumerable<PackageDto>> GetAllAsync();
    Task<IEnumerable<PackageDto>> GetActiveAsync();
    Task<PackageDto?> GetByIdAsync(int id);
    Task<PackageDto> CreateAsync(CreatePackageDto dto);
    Task<PackageDto> UpdateAsync(int id, UpdatePackageDto dto);
    Task DeleteAsync(int id);
}
