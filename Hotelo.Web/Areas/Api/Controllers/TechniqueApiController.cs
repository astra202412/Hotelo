using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.Technique;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/technique")]
[ApiController]
[Authorize]
public class TechniqueApiController : ControllerBase
{
    private readonly ITechniqueService _service;
    public TechniqueApiController(ITechniqueService service) => _service = service;

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
        => Ok(ApiResponse<object>.Ok(await _service.GetDashboardAsync()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTechDto dto)
        => Ok(ApiResponse<object>.Ok(await _service.CreateAsync(dto), "Intervention creee"));

    [HttpPost("{id}/start")]
    public async Task<IActionResult> Start(int id)
    {
        await _service.StartAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Intervention demarree"));
    }

    [HttpPost("{id}/close")]
    public async Task<IActionResult> Close(int id, [FromBody] CloseInterventionDto dto)
    {
        await _service.CloseAsync(id, dto.Resolution, dto.Cost);
        return Ok(ApiResponse<object>.Ok(new { id }, "Intervention fermee"));
    }
}

public class CloseInterventionDto
{
    public string Resolution { get; set; } = string.Empty;
    public decimal Cost { get; set; } = 0;
}
