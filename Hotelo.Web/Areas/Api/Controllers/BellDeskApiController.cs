using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.BellDesk;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/belldesk")]
[ApiController]
[Authorize]
public class BellDeskApiController : ControllerBase
{
    private readonly IBellDeskService _service;
    public BellDeskApiController(IBellDeskService service) => _service = service;

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
        => Ok(ApiResponse<object>.Ok(await _service.GetDashboardAsync()));

    [HttpPost("luggage")]
    public async Task<IActionResult> CreateLuggage([FromBody] CreateLuggageDto dto)
        => Ok(ApiResponse<object>.Ok(await _service.CreateLuggageAsync(dto), "Bagage enregistre"));

    [HttpPost("luggage/{id}/handle")]
    public async Task<IActionResult> HandleLuggage(int id)
    {
        await _service.HandleLuggageAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Bagage livre"));
    }

    [HttpPost("services")]
    public async Task<IActionResult> CreateService([FromBody] CreateGuestServiceDto dto)
        => Ok(ApiResponse<object>.Ok(await _service.CreateServiceAsync(dto), "Service cree"));

    [HttpPost("services/{id}/complete")]
    public async Task<IActionResult> CompleteService(int id)
    {
        await _service.CompleteServiceAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Service termine"));
    }
}
