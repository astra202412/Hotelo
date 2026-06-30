using Hotelo.Core.Entities.HR;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Repositories;

public class HRRepository : GenericRepository<Employee>, IHRRepository
{
    public HRRepository(HoteloDbContext context) : base(context) { }

    public async Task<IEnumerable<Employee>> GetAllWithDepartmentAsync()
        => await _context.Employees
            .Include(e => e.Department)
            .Where(e => e.IsActive)
            .OrderBy(e => e.Department.Name)
            .ThenBy(e => e.FullName)
            .ToListAsync();

    public async Task<Employee?> GetWithDepartmentAsync(int id)
        => await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
        => await _context.Employees
            .Include(e => e.Department)
            .Where(e => e.DepartmentId == departmentId && e.IsActive)
            .ToListAsync();

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        => await _context.Departments
            .Include(d => d.Employees.Where(e => e.IsActive))
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .ToListAsync();

    public async Task<Department?> GetDepartmentByIdAsync(int id)
        => await _context.Departments
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);
}
