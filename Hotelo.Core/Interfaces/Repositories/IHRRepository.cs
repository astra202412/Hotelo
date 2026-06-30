using Hotelo.Core.Entities.HR;

namespace Hotelo.Core.Interfaces.Repositories;

public interface IHRRepository : IGenericRepository<Employee>
{
    Task<IEnumerable<Employee>> GetAllWithDepartmentAsync();
    Task<Employee?> GetWithDepartmentAsync(int id);
    Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);
    Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    Task<Department?> GetDepartmentByIdAsync(int id);
}
