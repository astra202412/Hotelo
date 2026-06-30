using Hotelo.Core.DTOs.HR;
using Hotelo.Core.Entities.HR;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Core.Interfaces.Services;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Services;

public class HRService : IHRService
{
    private readonly IHRRepository _hrRepo;
    private readonly HoteloDbContext _context;

    public HRService(IHRRepository hrRepo, HoteloDbContext context)
    {
        _hrRepo = hrRepo;
        _context = context;
    }

    private static EmployeeDto ToDto(Employee e) => new()
    {
        Id = e.Id,
        FullName = e.FullName,
        DepartmentId = e.DepartmentId,
        DepartmentName = e.Department?.Name ?? "",
        UserId = e.UserId,
        HireDate = e.HireDate,
        ContractType = e.ContractType,
        BaseSalary = e.BaseSalary,
        IsActive = e.IsActive,
        YearsOfService = (int)((DateTime.Today - e.HireDate).TotalDays / 365)
    };

    private static DepartmentDto DeptToDto(Department d) => new()
    {
        Id = d.Id,
        Name = d.Name,
        Description = d.Description,
        IsActive = d.IsActive,
        EmployeeCount = d.Employees?.Count(e => e.IsActive) ?? 0
    };

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        => (await _hrRepo.GetAllWithDepartmentAsync()).Select(ToDto);

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var e = await _hrRepo.GetWithDepartmentAsync(id);
        return e == null ? null : ToDto(e);
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        => (await _hrRepo.GetAllDepartmentsAsync()).Select(DeptToDto);

    public async Task<HRDashboardDto> GetDashboardAsync()
    {
        var employees = (await _hrRepo.GetAllWithDepartmentAsync()).ToList();
        var departments = (await _hrRepo.GetAllDepartmentsAsync()).ToList();

        return new HRDashboardDto
        {
            TotalEmployees = employees.Count,
            ActiveEmployees = employees.Count(e => e.IsActive),
            TotalDepartments = departments.Count,
            AverageSalary = employees.Any() ? Math.Round(employees.Average(e => e.BaseSalary), 0) : 0,
            Departments = departments.Select(DeptToDto).ToList(),
            Employees = employees.Select(ToDto).ToList()
        };
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var emp = new Employee
        {
            FullName = dto.FullName,
            DepartmentId = dto.DepartmentId,
            UserId = dto.UserId,
            HireDate = dto.HireDate,
            ContractType = dto.ContractType,
            BaseSalary = dto.BaseSalary,
            IsActive = true
        };
        await _hrRepo.AddAsync(emp);
        return ToDto(await _hrRepo.GetWithDepartmentAsync(emp.Id) ?? emp);
    }

    public async Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
    {
        var emp = await _hrRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Employe {id} introuvable");
        emp.FullName = dto.FullName;
        emp.DepartmentId = dto.DepartmentId;
        emp.UserId = dto.UserId;
        emp.HireDate = dto.HireDate;
        emp.ContractType = dto.ContractType;
        emp.BaseSalary = dto.BaseSalary;
        emp.IsActive = dto.IsActive;
        await _hrRepo.UpdateAsync(emp);
        return ToDto(await _hrRepo.GetWithDepartmentAsync(id) ?? emp);
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        var emp = await _hrRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Employe {id} introuvable");
        emp.IsActive = false;
        await _hrRepo.UpdateAsync(emp);
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
    {
        var dept = new Department
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true
        };
        _context.Departments.Add(dept);
        await _context.SaveChangesAsync();
        return DeptToDto(dept);
    }
    public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
    {
        var d = await _context.Departments.FindAsync(id);
        if (d == null) return null;
        var count = await _context.Employees.CountAsync(e => e.DepartmentId == d.Id && e.IsActive);
        return new DepartmentDto { Id=d.Id, Name=d.Name, Description=d.Description, IsActive=d.IsActive, EmployeeCount=count };
    }

    public async Task<DepartmentDto> UpdateDepartmentAsync(int id, CreateDepartmentDto dto)
    {
        var d = await _context.Departments.FindAsync(id)
            ?? throw new Exception("Departement introuvable");
        d.Name        = dto.Name;
        d.Description = dto.Description;
        await _context.SaveChangesAsync();
        return new DepartmentDto { Id=d.Id, Name=d.Name, Description=d.Description, IsActive=d.IsActive };
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        var d = await _context.Departments.FindAsync(id);
        if (d != null) { d.IsActive = false; await _context.SaveChangesAsync(); }
    }
}