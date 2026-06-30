using Hotelo.Common.Helpers;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/reports")]
[ApiController]
[Authorize]
public class ReportsApiController : ControllerBase
{
    private readonly IReportService _service;
    public ReportsApiController(IReportService service) => _service = service;

    [HttpGet("direction")]
    public async Task<IActionResult> GetDirectionReport(
        int? year = null, int? month = null)
    {
        var y = year ?? DateTime.Today.Year;
        var m = month ?? DateTime.Today.Month;
        var report = await _service.GetDirectionReportAsync(y, m);
        return Ok(ApiResponse<object>.Ok(report));
    }
}
