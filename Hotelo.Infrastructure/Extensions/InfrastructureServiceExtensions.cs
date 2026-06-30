using Hotelo.Core.Entities.Identity;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Core.Interfaces.Services;
using Hotelo.Infrastructure.Data;
using Hotelo.Infrastructure.Repositories;
using Hotelo.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hotelo.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HoteloDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("HoteloConnection")));

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        })
        .AddEntityFrameworkStores<HoteloDbContext>()
        .AddDefaultTokenProviders();

        // Repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IGuestRepository, GuestRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IHousekeepingRepository, HousekeepingRepository>();
        services.AddScoped<IHRRepository, HRRepository>();

        // Services
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IGuestService, GuestService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IHousekeepingService, HousekeepingService>();
        services.AddScoped<IHRService, HRService>();
        services.AddScoped<ICommercialService, CommercialService>();
        services.AddScoped<IBellDeskService, BellDeskService>();
        services.AddScoped<ITechniqueService, TechniqueService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IExportService, ExportService>();
                services.AddScoped<IAdminService,       AdminService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IPackageService, PackageService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IBackupService, BackupService>();
        services.AddScoped<IBackupService, BackupService>();

        return services;
    }
}





