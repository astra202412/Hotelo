
using Hotelo.Core.Entities.System;
using Hotelo.Infrastructure.Data;

namespace Hotelo.Infrastructure.Data.SeedData;

public static class AdminSeed
{
    public static async Task SeedAsync(HoteloDbContext context)
    {
        if (context.AppModules.Any()) return;

        var modules = new List<AppModule>
        {
            new() { Name="Dashboard",     Code="DASHBOARD",    Icon="bi-speedometer2",     SortOrder=1 },
            new() { Name="FrontOffice",   Code="FRONTOFFICE",  Icon="bi-building",         SortOrder=2 },
            new() { Name="Reservations",  Code="RESERVATION",  Icon="bi-calendar-check",   SortOrder=3 },
            new() { Name="Packages",      Code="PACKAGES",     Icon="bi-gift",             SortOrder=4 },
            new() { Name="Finances",      Code="FINANCES",     Icon="bi-cash-stack",       SortOrder=5 },
            new() { Name="Housekeeping",  Code="HOUSEKEEPING", Icon="bi-brush",            SortOrder=6 },
            new() { Name="Technique",     Code="TECHNIQUE",    Icon="bi-tools",            SortOrder=7 },
            new() { Name="RH",            Code="HR",           Icon="bi-people",           SortOrder=8 },
            new() { Name="Commercial",    Code="COMMERCIAL",   Icon="bi-briefcase",        SortOrder=9 },
            new() { Name="BellDesk",      Code="BELLDESK",     Icon="bi-bell",             SortOrder=10 },
            new() { Name="Utilisateurs",  Code="USERS",        Icon="bi-person-gear",      SortOrder=11 },
            new() { Name="Administration",Code="ADMIN",        Icon="bi-shield-lock",      SortOrder=12 },
        };

        context.AppModules.AddRange(modules);
        await context.SaveChangesAsync();
    }
}
