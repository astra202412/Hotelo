namespace Hotelo.Common.Constants;

public static class RoleConstants
{
    public const string Admin = "Admin";
    public const string Directeur = "Directeur";
    public const string Receptionniste = "Receptionniste";
    public const string Comptable = "Comptable";
    public const string Gouvernante = "Gouvernante";
    public const string Technicien = "Technicien";
    public const string RH = "RH";
    public const string Commercial = "Commercial";
    public const string BellDesk = "BellDesk";

    public static readonly string[] AllRoles = {
        Admin, Directeur, Receptionniste, Comptable,
        Gouvernante, Technicien, RH, Commercial, BellDesk
    };
}