using Hotelo.Common.Enums;
using Hotelo.Core.Entities.FrontOffice;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Data.SeedData;

public static class FrontOfficeSeed
{
    public static async Task SeedAsync(HoteloDbContext context)
    {
        // Floors
        if (!await context.Floors.AnyAsync())
        {
            context.Floors.AddRange(
                new Floor { Number = 0, Name = "Rez-de-chaussee" },
                new Floor { Number = 1, Name = "1er Etage" },
                new Floor { Number = 2, Name = "2eme Etage" },
                new Floor { Number = 3, Name = "3eme Etage" }
            );
            await context.SaveChangesAsync();
        }

        // RoomTypes
        if (!await context.RoomTypes.AnyAsync())
        {
            context.RoomTypes.AddRange(
                new RoomType { Name = "Standard", BasePrice = 8000, MaxOccupancy = 2, IsActive = true, Description = "Chambre standard" },
                new RoomType { Name = "Double", BasePrice = 12000, MaxOccupancy = 2, IsActive = true, Description = "Chambre double" },
                new RoomType { Name = "Suite", BasePrice = 22000, MaxOccupancy = 3, IsActive = true, Description = "Suite deluxe" },
                new RoomType { Name = "Familiale", BasePrice = 16000, MaxOccupancy = 4, IsActive = true, Description = "Chambre familiale" },
                new RoomType { Name = "Junior Suite", BasePrice = 18000, MaxOccupancy = 2, IsActive = true, Description = "Junior Suite" }
            );
            await context.SaveChangesAsync();
        }

        // Rooms
        if (!await context.Rooms.AnyAsync())
        {
            var floors = await context.Floors.ToListAsync();
            var roomTypes = await context.RoomTypes.ToListAsync();

            var rdc = floors.First(f => f.Number == 0);
            var floor1 = floors.First(f => f.Number == 1);
            var floor2 = floors.First(f => f.Number == 2);
            var floor3 = floors.First(f => f.Number == 3);

            var standard = roomTypes.First(r => r.Name == "Standard");
            var doubleTy = roomTypes.First(r => r.Name == "Double");
            var suite = roomTypes.First(r => r.Name == "Suite");
            var familiale = roomTypes.First(r => r.Name == "Familiale");
            var junior = roomTypes.First(r => r.Name == "Junior Suite");

            var rooms = new List<Room>
            {
                // RDC
                new Room { RoomNumber="001", FloorId=rdc.Id,    RoomTypeId=standard.Id,  Status=RoomStatus.Libre,       IsActive=true },
                new Room { RoomNumber="002", FloorId=rdc.Id,    RoomTypeId=standard.Id,  Status=RoomStatus.Maintenance, IsActive=true },
                new Room { RoomNumber="003", FloorId=rdc.Id,    RoomTypeId=doubleTy.Id,  Status=RoomStatus.Libre,       IsActive=true },

                // 1er Etage
                new Room { RoomNumber="101", FloorId=floor1.Id, RoomTypeId=standard.Id,  Status=RoomStatus.Occupee,     IsActive=true },
                new Room { RoomNumber="102", FloorId=floor1.Id, RoomTypeId=standard.Id,  Status=RoomStatus.Libre,       IsActive=true },
                new Room { RoomNumber="103", FloorId=floor1.Id, RoomTypeId=doubleTy.Id,  Status=RoomStatus.ANettoyage,  IsActive=true },
                new Room { RoomNumber="104", FloorId=floor1.Id, RoomTypeId=doubleTy.Id,  Status=RoomStatus.Libre,       IsActive=true },
                new Room { RoomNumber="105", FloorId=floor1.Id, RoomTypeId=familiale.Id, Status=RoomStatus.Occupee,     IsActive=true },

                // 2eme Etage
                new Room { RoomNumber="201", FloorId=floor2.Id, RoomTypeId=standard.Id,  Status=RoomStatus.Libre,       IsActive=true },
                new Room { RoomNumber="202", FloorId=floor2.Id, RoomTypeId=standard.Id,  Status=RoomStatus.EnNettoyage, IsActive=true },
                new Room { RoomNumber="203", FloorId=floor2.Id, RoomTypeId=doubleTy.Id,  Status=RoomStatus.Libre,       IsActive=true },
                new Room { RoomNumber="204", FloorId=floor2.Id, RoomTypeId=doubleTy.Id,  Status=RoomStatus.Reservee,    IsActive=true },
                new Room { RoomNumber="205", FloorId=floor2.Id, RoomTypeId=junior.Id,    Status=RoomStatus.Libre,       IsActive=true },

                // 3eme Etage
                new Room { RoomNumber="301", FloorId=floor3.Id, RoomTypeId=suite.Id,     Status=RoomStatus.Occupee,     IsActive=true },
                new Room { RoomNumber="302", FloorId=floor3.Id, RoomTypeId=suite.Id,     Status=RoomStatus.Libre,       IsActive=true },
                new Room { RoomNumber="303", FloorId=floor3.Id, RoomTypeId=junior.Id,    Status=RoomStatus.Bloquee,     IsActive=true },
                new Room { RoomNumber="304", FloorId=floor3.Id, RoomTypeId=junior.Id,    Status=RoomStatus.Libre,       IsActive=true },
            };

            context.Rooms.AddRange(rooms);
            await context.SaveChangesAsync();
        }
    }
}
