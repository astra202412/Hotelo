using Hotelo.Core.Entities.Reservation;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Data.SeedData;

public static class PackageSeed
{
    public static async Task SeedAsync(HoteloDbContext context)
    {
        if (await context.Packages.AnyAsync()) return;

        var packages = new List<Package>
        {
            new Package {
                Name = "Sejour Standard",
                Description = "Hebergement seul sans extras",
                AdditionalPrice = 0,
                IncludesBreakfast = false,
                IncludesParking = false,
                IncludesSpa = false,
                IncludesAirportTransfer = false,
                IsActive = true, CreatedAt = DateTime.UtcNow
            },
            new Package {
                Name = "Petit-Dejeuner Inclus",
                Description = "Hebergement + petit-dejeuner buffet pour 2 personnes",
                AdditionalPrice = 1500,
                IncludesBreakfast = true,
                IncludesParking = false,
                IncludesSpa = false,
                IncludesAirportTransfer = false,
                IsActive = true, CreatedAt = DateTime.UtcNow
            },
            new Package {
                Name = "Business Pro",
                Description = "Petit-dejeuner + Parking + Wi-Fi premium",
                AdditionalPrice = 2500,
                IncludesBreakfast = true,
                IncludesParking = true,
                IncludesSpa = false,
                IncludesAirportTransfer = false,
                MinNights = 2,
                IsActive = true, CreatedAt = DateTime.UtcNow
            },
            new Package {
                Name = "Sejour Romance",
                Description = "Petit-dejeuner + Spa + decoration chambre",
                AdditionalPrice = 4500,
                IncludesBreakfast = true,
                IncludesParking = false,
                IncludesSpa = true,
                IncludesAirportTransfer = false,
                MinNights = 2,
                IsActive = true, CreatedAt = DateTime.UtcNow
            },
            new Package {
                Name = "All Inclusive",
                Description = "Tout inclus : petit-dejeuner, spa, parking, transfert aeroport",
                AdditionalPrice = 8000,
                IncludesBreakfast = true,
                IncludesParking = true,
                IncludesSpa = true,
                IncludesAirportTransfer = true,
                MinNights = 3,
                IsActive = true, CreatedAt = DateTime.UtcNow
            },
        };
        context.Packages.AddRange(packages);
        await context.SaveChangesAsync();
    }
}
