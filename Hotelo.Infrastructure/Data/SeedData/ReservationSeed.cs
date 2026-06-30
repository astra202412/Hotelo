using Hotelo.Common.Enums;
using Hotelo.Core.Entities.Reservation;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Data.SeedData;

public static class ReservationSeed
{
    public static async Task SeedAsync(HoteloDbContext context)
    {
        if (await context.Guests.AnyAsync()) return;

        // Guests
        var guests = new List<Guest>
        {
            new Guest { FullName="Ahmed Benali",    Passport="P123456", Nationality="Algerien", Phone="0661234567", Email="ahmed@mail.com",   VIPLevel=2, CreatedAt=DateTime.UtcNow },
            new Guest { FullName="Sara Mansouri",   Passport="P234567", Nationality="Algerien", Phone="0672345678", Email="sara@mail.com",    VIPLevel=1, CreatedAt=DateTime.UtcNow },
            new Guest { FullName="Karim Hadj",      Passport="P345678", Nationality="Algerien", Phone="0683456789", Email="karim@mail.com",   VIPLevel=0, CreatedAt=DateTime.UtcNow },
            new Guest { FullName="Marie Dupont",    Passport="F456789", Nationality="Francais", Phone="0033612345", Email="marie@mail.com",   VIPLevel=0, CreatedAt=DateTime.UtcNow },
            new Guest { FullName="John Smith",      Passport="GB56789", Nationality="Anglais",  Phone="0044712345", Email="john@mail.com",    VIPLevel=3, CreatedAt=DateTime.UtcNow },
        };
        context.Guests.AddRange(guests);
        await context.SaveChangesAsync();

        // Reservations
        var today = DateTime.Today;
        var rooms = await context.Rooms.Include(r => r.RoomType).ToListAsync();

        var reservations = new List<Reservation>
        {
            new Reservation {
                GuestId=guests[0].Id, RoomId=rooms.First(r=>r.RoomNumber=="101").Id,
                CheckIn=today, CheckOut=today.AddDays(3),
                Adults=2, Status=ReservationStatus.CheckedIn,
                RoomRate=8000, TotalAmount=24000,
                ConfirmationCode="HTL20250001", CreatedAt=DateTime.UtcNow
            },
            new Reservation {
                GuestId=guests[1].Id, RoomId=rooms.First(r=>r.RoomNumber=="105").Id,
                CheckIn=today, CheckOut=today.AddDays(2),
                Adults=2, Children=1, Status=ReservationStatus.CheckedIn,
                RoomRate=16000, TotalAmount=32000,
                ConfirmationCode="HTL20250002", CreatedAt=DateTime.UtcNow
            },
            new Reservation {
                GuestId=guests[2].Id, RoomId=rooms.First(r=>r.RoomNumber=="204").Id,
                CheckIn=today.AddDays(1), CheckOut=today.AddDays(4),
                Adults=1, Status=ReservationStatus.Confirmee,
                RoomRate=12000, TotalAmount=36000,
                ConfirmationCode="HTL20250003", CreatedAt=DateTime.UtcNow
            },
            new Reservation {
                GuestId=guests[3].Id, RoomId=rooms.First(r=>r.RoomNumber=="301").Id,
                CheckIn=today, CheckOut=today.AddDays(5),
                Adults=2, Status=ReservationStatus.CheckedIn,
                RoomRate=22000, TotalAmount=110000,
                ConfirmationCode="HTL20250004", CreatedAt=DateTime.UtcNow
            },
            new Reservation {
                GuestId=guests[4].Id, RoomId=rooms.First(r=>r.RoomNumber=="205").Id,
                CheckIn=today.AddDays(2), CheckOut=today.AddDays(7),
                Adults=1, Status=ReservationStatus.Confirmee,
                RoomRate=18000, TotalAmount=90000,
                ConfirmationCode="HTL20250005", CreatedAt=DateTime.UtcNow
            },
        };
        context.Reservations.AddRange(reservations);
        await context.SaveChangesAsync();
    }
}
