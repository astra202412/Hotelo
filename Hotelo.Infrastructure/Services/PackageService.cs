using Hotelo.Core.DTOs.Package;
using Hotelo.Core.Entities.Reservation;
using Hotelo.Core.Interfaces.Services;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Services;

public class PackageService : IPackageService
{
    private readonly HoteloDbContext _context;

    public PackageService(HoteloDbContext context)
    {
        _context = context;
    }

    private static PackageDto ToDto(Package p)
    {
        var includes = new List<string>();
        if (p.IncludesBreakfast) includes.Add("Petit-dejeuner");
        if (p.IncludesParking) includes.Add("Parking");
        if (p.IncludesSpa) includes.Add("Spa");
        if (p.IncludesAirportTransfer) includes.Add("Transfert Aeroport");

        return new PackageDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            AdditionalPrice = p.AdditionalPrice,
            IncludesBreakfast = p.IncludesBreakfast,
            IncludesParking = p.IncludesParking,
            IncludesSpa = p.IncludesSpa,
            IncludesAirportTransfer = p.IncludesAirportTransfer,
            MinNights = p.MinNights,
            IsActive = p.IsActive,
            IncludesLabel = includes.Any() ? string.Join(", ", includes) : "Hebergement seul"
        };
    }

    public async Task<IEnumerable<PackageDto>> GetAllAsync()
        => (await _context.Packages.OrderBy(p => p.Name).ToListAsync()).Select(ToDto);

    public async Task<IEnumerable<PackageDto>> GetActiveAsync()
        => (await _context.Packages.Where(p => p.IsActive).OrderBy(p => p.Name).ToListAsync()).Select(ToDto);

    public async Task<PackageDto?> GetByIdAsync(int id)
    {
        var p = await _context.Packages.FindAsync(id);
        return p == null ? null : ToDto(p);
    }

    public async Task<PackageDto> CreateAsync(CreatePackageDto dto)
    {
        var pkg = new Package
        {
            Name = dto.Name,
            Description = dto.Description,
            AdditionalPrice = dto.AdditionalPrice,
            IncludesBreakfast = dto.IncludesBreakfast,
            IncludesParking = dto.IncludesParking,
            IncludesSpa = dto.IncludesSpa,
            IncludesAirportTransfer = dto.IncludesAirportTransfer,
            MinNights = dto.MinNights,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        _context.Packages.Add(pkg);
        await _context.SaveChangesAsync();
        return ToDto(pkg);
    }

    public async Task<PackageDto> UpdateAsync(int id, UpdatePackageDto dto)
    {
        var pkg = await _context.Packages.FindAsync(id)
            ?? throw new KeyNotFoundException($"Package {id} introuvable");
        pkg.Name = dto.Name;
        pkg.Description = dto.Description;
        pkg.AdditionalPrice = dto.AdditionalPrice;
        pkg.IncludesBreakfast = dto.IncludesBreakfast;
        pkg.IncludesParking = dto.IncludesParking;
        pkg.IncludesSpa = dto.IncludesSpa;
        pkg.IncludesAirportTransfer = dto.IncludesAirportTransfer;
        pkg.MinNights = dto.MinNights;
        pkg.IsActive = dto.IsActive;
        await _context.SaveChangesAsync();
        return ToDto(pkg);
    }

    public async Task DeleteAsync(int id)
    {
        var pkg = await _context.Packages.FindAsync(id)
            ?? throw new KeyNotFoundException($"Package {id} introuvable");
        pkg.IsActive = false;
        await _context.SaveChangesAsync();
    }
}
