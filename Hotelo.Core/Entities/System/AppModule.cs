namespace Hotelo.Core.Entities.System;

// Module = ecran/fonctionnalite du systeme
public class AppModule : Hotelo.Core.Entities.Common.BaseEntity
{
    public string  Name        { get; set; } = string.Empty;
    public string  Code        { get; set; } = string.Empty; // Ex: FRONTOFFICE, RESERVATION
    public string? Description { get; set; }
    public string? Icon        { get; set; }
    public int     SortOrder   { get; set; }
    public virtual ICollection<UserAccess> UserAccesses { get; set; } = new List<UserAccess>();
}
