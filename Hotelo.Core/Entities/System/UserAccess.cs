namespace Hotelo.Core.Entities.System;

// Droits d acces : quelle fonction peut faire quoi sur quel module
public class UserAccess : Hotelo.Core.Entities.Common.BaseEntity
{
    public int    JobFunctionId { get; set; }
    public int    AppModuleId   { get; set; }
    public bool   CanView       { get; set; } = false;
    public bool   CanCreate     { get; set; } = false;
    public bool   CanEdit       { get; set; } = false;
    public bool   CanDelete     { get; set; } = false;
    public bool   CanExport     { get; set; } = false;
    public virtual JobFunction JobFunction { get; set; } = null!;
    public virtual AppModule   AppModule   { get; set; } = null!;
}
