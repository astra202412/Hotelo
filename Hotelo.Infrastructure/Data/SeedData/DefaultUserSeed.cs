using Hotelo.Common.Constants;
using Hotelo.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Hotelo.Infrastructure.Data.SeedData;

public static class DefaultUserSeed
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@hotelo.local";
        const string adminPassword = "Hotelo@2025!";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Administrateur",
                IsActive = true,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, RoleConstants.Admin);
        }
    }
}