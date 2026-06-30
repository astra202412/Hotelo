using Hotelo.Core.Entities.Common;
using Hotelo.Core.Entities.System;

namespace Hotelo.Core.Entities.HR;
public class Employee : AuditableEntity
{
    public string   FullName       { get; set; } = string.Empty;
    public int      DepartmentId   { get; set; }
    public int?     JobFunctionId  { get; set; }
    public string?  UserId         { get; set; }
    public DateTime HireDate       { get; set; }
    public string   ContractType   { get; set; } = "CDI";
    public decimal  BaseSalary     { get; set; }
    public bool     IsActive       { get; set; } = true;
    public virtual Department  Department  { get; set; } = null!;
    public virtual JobFunction? JobFunction { get; set; }
}