using Hotelo.Core.DTOs.Admin;

namespace Hotelo.Core.Interfaces.Services;

public interface IAdminService
{
    Task<IEnumerable<JobFunctionDto>>      GetAllFunctionsAsync();
    Task<IEnumerable<JobFunctionDto>>      GetFunctionsByDepartmentAsync(int departmentId);
    Task<JobFunctionDto>                   CreateFunctionAsync(CreateJobFunctionDto dto);
    Task                                   DeleteFunctionAsync(int id);
    Task<IEnumerable<AppModuleDto>>        GetAllModulesAsync();
    Task<AccessMatrixDto>                  GetAccessMatrixAsync();
    Task                                   UpdateAccessAsync(UpdateUserAccessDto dto);
    Task<IEnumerable<EmployeeWithUserDto>> GetEmployeesWithUserStatusAsync();
    Task<string>                           CreateUserFromEmployeeAsync(CreateUserFromEmployeeDto dto);
    Task                                   AssignFunctionToUserAsync(string userId, int functionId);
    Task                                   RemoveFunctionFromUserAsync(string userId, int functionId);
    Task<IEnumerable<UserJobFunctionDto>>  GetUserFunctionsAsync();
    Task<AccessMatrixDto>                  GetUserAccessMatrixAsync(string userId);
}