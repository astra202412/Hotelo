using Hotelo.Core.DTOs.HR;

namespace Hotelo.Core.Interfaces.Services;

public interface IHRService
{
    Task<HRDashboardDto> GetDashboardAsync();
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<EmployeeDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
    Task DeleteEmployeeAsync(int id);
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    Task<DepartmentDto?>          GetDepartmentByIdAsync(int id);
    Task<DepartmentDto>           UpdateDepartmentAsync(int id, CreateDepartmentDto dto);
    Task                          DeleteDepartmentAsync(int id);
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto);
}
