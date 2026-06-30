using Hotelo.Core.Entities.BellDesk;
using Hotelo.Core.Entities.Commercial;
using Hotelo.Core.Entities.Finances;
using Hotelo.Core.Entities.FrontOffice;
using Hotelo.Core.Entities.Housekeeping;
using Hotelo.Core.Entities.HR;
using Hotelo.Core.Entities.Identity;
using Hotelo.Core.Entities.Reservation;
using Hotelo.Core.Entities.Identity;
using Hotelo.Core.Entities.System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//----------------------------------------------------------------------------------------------------------------
namespace Hotelo.Infrastructure.Data;
//----------------------------------------------------------------------------------------------------------------
public class HoteloDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public HoteloDbContext(DbContextOptions<HoteloDbContext> options) : base(options) { }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Floor> Floors { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<HousekeepingTask> HousekeepingTasks { get; set; }
    public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<TechIntervention> TechInterventions { get; set; }
    public DbSet<Luggage> Luggages { get; set; }
    public DbSet<ConciergeService> GuestServices { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<JobFunction>     JobFunctions    { get; set; }
    public DbSet<AppModule>      AppModules      { get; set; }
    public DbSet<UserAccess>     UserAccesses    { get; set; }
    public DbSet<UserJobFunction> UserJobFunctions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Invoice
        builder.Entity<Invoice>().Property(i => i.TotalHT).HasColumnType("decimal(18,2)");
        builder.Entity<Invoice>().Property(i => i.TotalTTC).HasColumnType("decimal(18,2)");
        builder.Entity<Invoice>().Property(i => i.TVARate).HasColumnType("decimal(18,4)");
        builder.Entity<Invoice>().Property(i => i.TVAAmount).HasColumnType("decimal(18,2)");
        builder.Entity<Invoice>().Property(i => i.Discount).HasColumnType("decimal(18,2)");

        // Payment
        builder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");

        // Reservation
        builder.Entity<Reservation>().Property(r => r.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Entity<Reservation>().Property(r => r.RoomRate).HasColumnType("decimal(18,2)");
        builder.Entity<Reservation>().Property(r => r.PackagePrice).HasColumnType("decimal(18,2)");
        builder.Entity<Reservation>().Property(r => r.Discount).HasColumnType("decimal(18,2)");

        // RoomType
        builder.Entity<RoomType>().Property(r => r.BasePrice).HasColumnType("decimal(18,2)");
        builder.Entity<TechIntervention>().Property(t => t.Cost).HasColumnType("decimal(18,2)");
        builder.Entity<Company>().Property(c => c.DiscountRate).HasColumnType("decimal(18,2)");
        builder.Entity<Contract>().Property(c => c.RoomRate).HasColumnType("decimal(18,2)");
        builder.Entity<Contract>().Property(c => c.DiscountRate).HasColumnType("decimal(18,2)");

        // Package
        builder.Entity<Package>().Property(p => p.AdditionalPrice).HasColumnType("decimal(18,2)");

        // Employee
        builder.Entity<Employee>().Property(e => e.BaseSalary).HasColumnType("decimal(18,2)");

        // UserAccess — cle composite
        builder.Entity<UserAccess>()
            .HasOne(u => u.JobFunction)
            .WithMany(j => j.UserAccesses)
            .HasForeignKey(u => u.JobFunctionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserAccess>()
            .HasOne(u => u.AppModule)
            .WithMany(m => m.UserAccesses)
            .HasForeignKey(u => u.AppModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserJobFunction>()
            .HasKey(u => new { u.UserId, u.JobFunctionId });

        // Soft delete filters
        builder.Entity<Reservation>().HasQueryFilter(r => !r.IsDeleted);
        builder.Entity<Guest>().HasQueryFilter(g => !g.IsDeleted);
    }
}








