using Hotelo.Common.Constants;
using Hotelo.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Hotelo.Infrastructure.Data.SeedData;

public static class DefaultRolesSeed
{
    public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
    {
        foreach (var roleName in RoleConstants.AllRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
        }
    }
}