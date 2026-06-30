using Hotelo.Common.Helpers;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/dashboard")]
[ApiController]
[Authorize]
public class DashboardApiController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardApiController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetGlobal()
        => Ok(ApiResponse<object>.Ok(await _service.GetGlobalDashboardAsync()));

    [HttpGet("kpis")]
    public async Task<IActionResult> GetKpis()
    {
        var d = await _service.GetGlobalDashboardAsync();
        return Ok(ApiResponse<object>.Ok(new
        {
            d.TotalRooms,
            d.OccupiedRooms,
            d.FreeRooms,
            d.OccupancyRate,
            d.TodayArrivals,
            d.TodayDepartures,
            d.ActiveReservations,
            d.RevenueToday,
            d.RevenueMonth,
            d.UnpaidInvoices,
            d.PendingAmount
        }));
    }
}
