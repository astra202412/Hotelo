using Hotelo.Core.DTOs.Commercial;
using Hotelo.Core.Entities.Commercial;
using Hotelo.Core.Interfaces.Services;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Services;

public class CommercialService : ICommercialService
{
    private readonly HoteloDbContext _context;

    public CommercialService(HoteloDbContext context)
    {
        _context = context;
    }

    private static CompanyDto ToDto(Company c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Type = c.Type,
        Address = c.Address,
        Phone = c.Phone,
        Email = c.Email,
        ContactPerson = c.ContactPerson,
        DiscountRate = c.DiscountRate,
        IsActive = c.IsActive,
        Notes = c.Notes,
        ContractsCount = c.Contracts?.Count ?? 0
    };

    private static ContractDto ContractToDto(Contract c) => new()
    {
        Id = c.Id,
        CompanyId = c.CompanyId,
        CompanyName = c.Company?.Name ?? "",
        Reference = c.Reference,
        StartDate = c.StartDate,
        EndDate = c.EndDate,
        RoomRate = c.RoomRate,
        DiscountRate = c.DiscountRate,
        RoomsQuota = c.RoomsQuota,
        IsActive = c.IsActive,
        Notes = c.Notes
    };

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        => (await _context.Companies
            .Include(c => c.Contracts)
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync()).Select(ToDto);

    public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
    {
        var c = await _context.Companies
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.Id == id);
        return c == null ? null : ToDto(c);
    }

    public async Task<IEnumerable<ContractDto>> GetAllContractsAsync()
        => (await _context.Contracts
            .Include(c => c.Company)
            .OrderByDescending(c => c.StartDate)
            .ToListAsync()).Select(ContractToDto);

    public async Task<CommercialDashboardDto> GetDashboardAsync()
    {
        var companies = await _context.Companies
            .Include(c => c.Contracts)
            .Where(c => c.IsActive)
            .ToListAsync();

        var contracts = await _context.Contracts
            .Include(c => c.Company)
            .ToListAsync();

        var active = contracts.Count(c => c.IsActive && DateTime.Today <= c.EndDate);
        var expired = contracts.Count(c => DateTime.Today > c.EndDate);
        var avgDisc = companies.Any() ? Math.Round(companies.Average(c => c.DiscountRate), 1) : 0;

        return new CommercialDashboardDto
        {
            TotalCompanies = companies.Count,
            ActiveContracts = active,
            ExpiredContracts = expired,
            AverageDiscount = avgDisc,
            Companies = companies.Select(ToDto).ToList(),
            Contracts = contracts.Select(ContractToDto).ToList()
        };
    }

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto dto)
    {
        var company = new Company
        {
            Name = dto.Name,
            Type = dto.Type,
            Address = dto.Address,
            Phone = dto.Phone,
            Email = dto.Email,
            ContactPerson = dto.ContactPerson,
            DiscountRate = dto.DiscountRate,
            IsActive = true,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return ToDto(company);
    }

    public async Task DeleteCompanyAsync(int id)
    {
        var c = await _context.Companies.FindAsync(id);
        if (c != null) { c.IsActive = false; await _context.SaveChangesAsync(); }
    }

    public async Task<ContractDto> CreateContractAsync(CreateContractDto dto)
    {
        var contract = new Contract
        {
            CompanyId = dto.CompanyId,
            Reference = $"CTR{DateTime.Now:yyyyMMddHHmmss}",
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            RoomRate = dto.RoomRate,
            DiscountRate = dto.DiscountRate,
            RoomsQuota = dto.RoomsQuota,
            IsActive = true,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };
        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();
        return ContractToDto(await _context.Contracts
            .Include(c => c.Company)
            .FirstAsync(c => c.Id == contract.Id));
    }
}
