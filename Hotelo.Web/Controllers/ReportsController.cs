using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task<IActionResult> Index(int? year, int? month)
    {
        var y = year ?? DateTime.Today.Year;
        var m = month ?? DateTime.Today.Month;
        var report = await _reportService.GetDirectionReportAsync(y, m);
        ViewBag.Year = y;
        ViewBag.Month = m;
        return View(report);
    }
}
