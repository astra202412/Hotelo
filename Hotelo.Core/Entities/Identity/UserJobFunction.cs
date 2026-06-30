namespace Hotelo.Core.Entities.Identity;

// Lien entre utilisateur et sa fonction
public class UserJobFunction
{
    public string UserId        { get; set; } = string.Empty;
    public int    JobFunctionId { get; set; }
    public virtual ApplicationUser  User        { get; set; } = null!;
    public virtual Hotelo.Core.Entities.System.JobFunction JobFunction { get; set; } = null!;
}
