using Hangfire;
using Hangfire;
using Hotelo.Infrastructure.Data;
using Hotelo.Infrastructure.Data.SeedData;
using Hotelo.Infrastructure.Extensions;
using Hotelo.Web.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<Hotelo.Web.Jobs.BackupJob>();
builder.Services.AddScoped<Hotelo.Web.Jobs.BackupJob>();
builder.Services.AddScoped<Hotelo.Core.Interfaces.Services.IRealtimeNotifier,
                            Hotelo.Web.Services.SignalRNotifier>();
builder.Services.AddScoped<Hotelo.Web.Jobs.BackupJob>();
builder.Services.AddScoped<Hotelo.Web.Jobs.BackupJob>();
builder.Services.AddScoped<Hotelo.Core.Interfaces.Services.IRealtimeNotifier,
                            Hotelo.Web.Services.SignalRNotifier>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSession(o => { o.IdleTimeout = TimeSpan.FromMinutes(30); o.Cookie.HttpOnly = true; });
builder.Services.AddEndpointsApiExplorer();

// Hangfire
builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("HoteloConnection")));
builder.Services.AddHangfireServer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "HOTELO API", Version = "v1" }));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var ctx = scope.ServiceProvider.GetRequiredService<HoteloDbContext>();
        await ctx.Database.EnsureCreatedAsync();
        var rm = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Hotelo.Core.Entities.Identity.ApplicationRole>>();
        var um = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Hotelo.Core.Entities.Identity.ApplicationUser>>();
        await DefaultRolesSeed.SeedAsync(rm);
        await DefaultUserSeed.SeedAsync(um);
        await Hotelo.Infrastructure.Data.SeedData.FrontOfficeSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.ReservationSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.HRSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.PackageSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.PackageSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.ReservationSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.HRSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.PackageSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.PackageSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.ReservationSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.HRSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.PackageSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.PackageSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
        await Hotelo.Infrastructure.Data.SeedData.AdminSeed.SeedAsync(ctx);
    }
    catch (Exception ex) { Log.Error(ex, "Erreur initialisation DB"); }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HOTELO API v1"));
    app.UseDeveloperExceptionPage();
}
else { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseHangfireDashboard("/hangfire", new Hangfire.DashboardOptions
{
    DashboardTitle = "HOTELO — Jobs Planifies",
    Authorization = new[] { new Hangfire.Dashboard.LocalRequestsOnlyAuthorizationFilter() }
});
app.UseAuthorization();
app.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");


// Exclure les RazorPages Identity generees par le template
// app.MapRazorPages();

app.MapHub<Hotelo.Web.Hubs.NotificationHub>("/hubs/notifications");
app.MapHangfireDashboard();

// Job backup quotidien a 02h00
Hangfire.RecurringJob.AddOrUpdate<Hotelo.Web.Jobs.BackupJob>(
    "daily-backup",
    job => job.ExecuteAsync(),
    "0 2 * * *");
app.Run();




















