using Hotelo.Core.DTOs.Admin;
using Hotelo.Core.Entities.HR;
using Hotelo.Core.Entities.Identity;
using Hotelo.Core.Entities.System;
using Hotelo.Core.Interfaces.Services;
using Hotelo.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly HoteloDbContext             _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminService(HoteloDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context     = context;
        _userManager = userManager;
    }

    // â”€â”€ Fonctions â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
    public async Task<IEnumerable<JobFunctionDto>> GetAllFunctionsAsync()
        => await _context.JobFunctions
            .Include(f => f.Department)
            .Where(f => f.IsActive)
            .OrderBy(f => f.Department.Name).ThenBy(f => f.Name)
            .Select(f => new JobFunctionDto
            {
                Id             = f.Id,
                Name           = f.Name,
                Description    = f.Description ?? string.Empty,
                DepartmentId   = f.DepartmentId,
                DepartmentName = f.Department.Name,
                IsActive       = f.IsActive,
                UsersCount     = _context.UserJobFunctions.Count(u => u.JobFunctionId == f.Id)
            }).ToListAsync();

    public async Task<IEnumerable<JobFunctionDto>> GetFunctionsByDepartmentAsync(int departmentId)
        => await _context.JobFunctions
            .Include(f => f.Department)
            .Where(f => f.IsActive && f.DepartmentId == departmentId)
            .OrderBy(f => f.Name)
            .Select(f => new JobFunctionDto
            {
                Id=f.Id, Name=f.Name, DepartmentId=f.DepartmentId,
                DepartmentName=f.Department.Name, IsActive=f.IsActive
            }).ToListAsync();

    public async Task<JobFunctionDto> CreateFunctionAsync(CreateJobFunctionDto dto)
    {
        var func = new JobFunction
            { Name=dto.Name, Description=dto.Description, DepartmentId=dto.DepartmentId, IsActive=true };
        _context.JobFunctions.Add(func);
        await _context.SaveChangesAsync();
        var modules = await _context.AppModules.ToListAsync();
        _context.UserAccesses.AddRange(modules.Select(m => new UserAccess
            { JobFunctionId=func.Id, AppModuleId=m.Id }));
        await _context.SaveChangesAsync();
        return new JobFunctionDto { Id=func.Id, Name=func.Name, DepartmentId=func.DepartmentId, IsActive=true };
    }

    public async Task DeleteFunctionAsync(int id)
    {
        var f = await _context.JobFunctions.FindAsync(id);
        if (f != null) { f.IsActive = false; await _context.SaveChangesAsync(); }
    }

    // â”€â”€ Modules â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
    public async Task<IEnumerable<AppModuleDto>> GetAllModulesAsync()
        => await _context.AppModules.OrderBy(m => m.SortOrder)
            .Select(m => new AppModuleDto
                { Id=m.Id, Name=m.Name, Code=m.Code, Description=m.Description ?? string.Empty, Icon=m.Icon, SortOrder=m.SortOrder })
            .ToListAsync();

    // â”€â”€ Matrice globale â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
    public async Task<AccessMatrixDto> GetAccessMatrixAsync()
    {
        var modules   = await _context.AppModules.OrderBy(m => m.SortOrder).ToListAsync();
        var functions = await _context.JobFunctions.Include(f => f.Department)
            .Where(f => f.IsActive).OrderBy(f => f.Department.Name).ThenBy(f => f.Name).ToListAsync();
        var accesses  = await _context.UserAccesses
            .Include(a => a.JobFunction).Include(a => a.AppModule).ToListAsync();
        return new AccessMatrixDto
        {
            Modules   = modules.Select(m => new AppModuleDto
                { Id=m.Id, Name=m.Name, Code=m.Code, Icon=m.Icon, SortOrder=m.SortOrder }).ToList(),
            Functions = functions.Select(f => new JobFunctionDto
                { Id=f.Id, Name=f.Name, DepartmentName=f.Department?.Name ?? string.Empty,
                  DepartmentId=f.DepartmentId, IsActive=f.IsActive }).ToList(),
            AccessRules = accesses.Select(a => new UserAccessDto
            {
                Id=a.Id, JobFunctionId=a.JobFunctionId, JobFunction=a.JobFunction?.Name ?? string.Empty,
                AppModuleId=a.AppModuleId, AppModule=a.AppModule?.Name ?? string.Empty,
                CanView=a.CanView, CanCreate=a.CanCreate, CanEdit=a.CanEdit, CanDelete=a.CanDelete, CanExport=a.CanExport
            }).ToList()
        };
    }

    // â”€â”€ Matrice par utilisateur â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
    public async Task<AccessMatrixDto> GetUserAccessMatrixAsync(string userId)
    {
        var functionIds = await _context.UserJobFunctions
            .Where(u => u.UserId == userId).Select(u => u.JobFunctionId).ToListAsync();
        var modules   = await _context.AppModules.OrderBy(m => m.SortOrder).ToListAsync();
        var functions = await _context.JobFunctions.Include(f => f.Department)
            .Where(f => functionIds.Contains(f.Id)).ToListAsync();
        var accesses  = await _context.UserAccesses
            .Include(a => a.JobFunction).Include(a => a.AppModule)
            .Where(a => functionIds.Contains(a.JobFunctionId)).ToListAsync();
        return new AccessMatrixDto
        {
            Modules   = modules.Select(m => new AppModuleDto { Id=m.Id, Name=m.Name, Code=m.Code, Icon=m.Icon }).ToList(),
            Functions = functions.Select(f => new JobFunctionDto
                { Id=f.Id, Name=f.Name, DepartmentName=f.Department?.Name ?? string.Empty }).ToList(),
            AccessRules = accesses.Select(a => new UserAccessDto
            {
                Id=a.Id, JobFunctionId=a.JobFunctionId, AppModuleId=a.AppModuleId,
                CanView=a.CanView, CanCreate=a.CanCreate, CanEdit=a.CanEdit, CanDelete=a.CanDelete, CanExport=a.CanExport
            }).ToList()
        };
    }

    public async Task UpdateAccessAsync(UpdateUserAccessDto dto)
    {
        var a = await _context.UserAccesses.FirstOrDefaultAsync(
            x => x.JobFunctionId == dto.JobFunctionId && x.AppModuleId == dto.AppModuleId);
        if (a == null)
        {
            a = new UserAccess { JobFunctionId=dto.JobFunctionId, AppModuleId=dto.AppModuleId };
            _context.UserAccesses.Add(a);
        }
        a.CanView=dto.CanView; a.CanCreate=dto.CanCreate; a.CanEdit=dto.CanEdit;
        a.CanDelete=dto.CanDelete; a.CanExport=dto.CanExport;
        await _context.SaveChangesAsync();
    }

    // â”€â”€ Flux Employe -> Utilisateur -> Droits â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
    public async Task<IEnumerable<EmployeeWithUserDto>> GetEmployeesWithUserStatusAsync()
        => await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.JobFunction)
            .Where(e => e.IsActive)
            .OrderBy(e => e.Department.Name).ThenBy(e => e.FullName)
            .Select(e => new EmployeeWithUserDto
            {
                EmployeeId      = e.Id,
                FullName        = e.FullName,
                DepartmentName  = e.Department.Name,
                DepartmentId    = e.DepartmentId,
                JobFunctionId   = e.JobFunctionId,
                JobFunctionName = e.JobFunction != null ? e.JobFunction.Name : string.Empty,
                UserId          = e.UserId,
                UserName        = e.UserId != null
                    ? _context.Users.Where(u => u.Id == e.UserId).Select(u => u.UserName).FirstOrDefault()
                    : null,
                HasUser  = e.UserId != null,
                IsActive = e.IsActive
            }).ToListAsync();

    public async Task<string> CreateUserFromEmployeeAsync(CreateUserFromEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(dto.EmployeeId)
            ?? throw new Exception("Employe introuvable");
        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email    = dto.Email,
            FullName = employee.FullName,
            IsActive = true
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        if (!string.IsNullOrEmpty(dto.Role))
            await _userManager.AddToRoleAsync(user, dto.Role);
        employee.UserId        = user.Id;
        employee.JobFunctionId = dto.JobFunctionId > 0 ? dto.JobFunctionId : null;
        if (dto.JobFunctionId > 0)
            _context.UserJobFunctions.Add(new UserJobFunction { UserId=user.Id, JobFunctionId=dto.JobFunctionId });
        await _context.SaveChangesAsync();
        return user.Id;
    }

    public async Task AssignFunctionToUserAsync(string userId, int functionId)
    {
        var exists = await _context.UserJobFunctions
            .AnyAsync(u => u.UserId == userId && u.JobFunctionId == functionId);
        if (!exists)
        {
            _context.UserJobFunctions.Add(new UserJobFunction { UserId=userId, JobFunctionId=functionId });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveFunctionFromUserAsync(string userId, int functionId)
    {
        var ujf = await _context.UserJobFunctions
            .FirstOrDefaultAsync(u => u.UserId == userId && u.JobFunctionId == functionId);
        if (ujf != null) { _context.UserJobFunctions.Remove(ujf); await _context.SaveChangesAsync(); }
    }

    public async Task<IEnumerable<UserJobFunctionDto>> GetUserFunctionsAsync()
        => await _context.UserJobFunctions
            .Include(u => u.User)
            .Include(u => u.JobFunction).ThenInclude(f => f.Department)
            .Select(u => new UserJobFunctionDto
            {
                UserId=u.UserId, UserName=u.User.UserName ?? string.Empty,
                FullName=u.User.FullName, JobFunctionId=u.JobFunctionId,
                JobFunctionName=u.JobFunction.Name, DepartmentName=u.JobFunction.Department.Name
            }).ToListAsync();
}