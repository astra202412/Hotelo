namespace Hotelo.Core.DTOs.HR;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int EmployeeCount { get; set; }
}

public class EmployeeDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public DateTime HireDate { get; set; }
    public string ContractType { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; }
    public bool IsActive { get; set; }
    public int YearsOfService { get; set; }
}

public class CreateEmployeeDto
{
    public string FullName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string? UserId { get; set; }
    public DateTime HireDate { get; set; }
    public string ContractType { get; set; } = "CDI";
    public decimal BaseSalary { get; set; }
}

public class UpdateEmployeeDto : CreateEmployeeDto
{
    public bool IsActive { get; set; } = true;
}

public class CreateDepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class HRDashboardDto
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int TotalDepartments { get; set; }
    public decimal AverageSalary { get; set; }
    public List<DepartmentDto> Departments { get; set; } = new();
    public List<EmployeeDto> Employees { get; set; } = new();
}
