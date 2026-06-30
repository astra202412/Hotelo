using Hotelo.Core.Entities.Common;
using Hotelo.Core.Entities.HR;
using Hotelo.Core.Entities.Identity;

namespace Hotelo.Core.Entities.System;

public class JobFunction : BaseEntity
{
    public string  Name         { get; set; } = string.Empty;
    public string? Description  { get; set; }
    public int     DepartmentId { get; set; }
    public bool    IsActive     { get; set; } = true;
    public virtual Department                    Department       { get; set; } = null!;
    public virtual ICollection<UserAccess>       UserAccesses     { get; set; } = new List<UserAccess>();
    public virtual ICollection<UserJobFunction>  UserJobFunctions { get; set; } = new List<UserJobFunction>();
}