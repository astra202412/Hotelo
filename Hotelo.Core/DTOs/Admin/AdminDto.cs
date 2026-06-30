namespace Hotelo.Core.DTOs.Admin;

public class JobFunctionDto
{
    public int    Id             { get; set; }
    public string Name           { get; set; } = string.Empty;
    public string Description    { get; set; } = string.Empty;
    public int    DepartmentId   { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public bool   IsActive       { get; set; }
    public int    UsersCount     { get; set; }
}

public class CreateJobFunctionDto
{
    public string Name         { get; set; } = string.Empty;
    public string Description  { get; set; } = string.Empty;
    public int    DepartmentId { get; set; }
}

public class AppModuleDto
{
    public int    Id          { get; set; }
    public string Name        { get; set; } = string.Empty;
    public string Code        { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon        { get; set; } = string.Empty;
    public int    SortOrder   { get; set; }
}

public class UserAccessDto
{
    public int    Id            { get; set; }
    public int    JobFunctionId { get; set; }
    public string JobFunction   { get; set; } = string.Empty;
    public int    AppModuleId   { get; set; }
    public string AppModule     { get; set; } = string.Empty;
    public bool   CanView       { get; set; }
    public bool   CanCreate     { get; set; }
    public bool   CanEdit       { get; set; }
    public bool   CanDelete     { get; set; }
    public bool   CanExport     { get; set; }
}

public class AccessMatrixDto
{
    public List<AppModuleDto>   Modules     { get; set; } = new();
    public List<JobFunctionDto> Functions   { get; set; } = new();
    public List<UserAccessDto>  AccessRules { get; set; } = new();
}

public class UpdateUserAccessDto
{
    public int  JobFunctionId { get; set; }
    public int  AppModuleId   { get; set; }
    public bool CanView       { get; set; }
    public bool CanCreate     { get; set; }
    public bool CanEdit       { get; set; }
    public bool CanDelete     { get; set; }
    public bool CanExport     { get; set; }
}

public class UserJobFunctionDto
{
    public string UserId          { get; set; } = string.Empty;
    public string UserName        { get; set; } = string.Empty;
    public string FullName        { get; set; } = string.Empty;
    public int    JobFunctionId   { get; set; }
    public string JobFunctionName { get; set; } = string.Empty;
    public string DepartmentName  { get; set; } = string.Empty;
}

public class CreateUserFromEmployeeDto
{
    public int    EmployeeId    { get; set; }
    public string UserName      { get; set; } = string.Empty;
    public string Email         { get; set; } = string.Empty;
    public string Password      { get; set; } = string.Empty;
    public int    JobFunctionId { get; set; }
    public string Role          { get; set; } = string.Empty;
}

public class EmployeeWithUserDto
{
    public int     EmployeeId      { get; set; }
    public string  FullName        { get; set; } = string.Empty;
    public string  DepartmentName  { get; set; } = string.Empty;
    public int     DepartmentId    { get; set; }
    public int?    JobFunctionId   { get; set; }
    public string  JobFunctionName { get; set; } = string.Empty;
    public string? UserId          { get; set; }
    public string? UserName        { get; set; }
    public bool    HasUser         { get; set; }
    public bool    IsActive        { get; set; }
}