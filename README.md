# HOTELO HMS

## Stack
- C# / .NET 8.0 / ASP.NET Core MVC
- Entity Framework Core 8
- SQL Server : ASTRA\SQLEXPRESS / hoteloDB / Windows Auth

## Projets
- Hotelo.Web        : MVC, Areas, API REST, SignalR
- Hotelo.Core       : Entites, Interfaces, DTOs
- Hotelo.Infrastructure : EF Core, DbContext, Repositories
- Hotelo.Common     : Constantes, Enums, Helpers

## Modules (Areas)
FrontOffice / Reservation / Finances / Housekeeping / Commercial / BellDesk / Technique / HR

## Demarrage
dotnet ef migrations add InitialCreate --project Hotelo.Infrastructure --startup-project Hotelo.Web
dotnet ef database update --project Hotelo.Infrastructure --startup-project Hotelo.Web
dotnet run --project Hotelo.Web

## Acces
- App    : https://localhost:5001
- Swagger: https://localhost:5001/swagger
- Admin  : admin@hotelo.local / Hotelo@2025!