using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var dashboard = await _dashboardService.GetGlobalDashboardAsync();
        return View(dashboard);
    }
}
