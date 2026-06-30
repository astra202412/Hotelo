using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.Housekeeping;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/housekeeping")]
[ApiController]
[Authorize]
public class HousekeepingApiController : ControllerBase
{
    private readonly IHousekeepingService _service;

    public HousekeepingApiController(IHousekeepingService service)
    {
        _service = service;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
        => Ok(ApiResponse<object>.Ok(await _service.GetDashboardAsync()));

    [HttpGet("tasks")]
    public async Task<IActionResult> GetTasks()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllTasksAsync()));

    [HttpPost("tasks")]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
    {
        var task = await _service.CreateTaskAsync(dto);
        return Ok(ApiResponse<object>.Ok(task, "Tache creee"));
    }

    [HttpPost("tasks/{id}/assign")]
    public async Task<IActionResult> AssignTask(int id, [FromBody] string userId)
    {
        await _service.AssignTaskAsync(id, userId);
        return Ok(ApiResponse<object>.Ok(new { id }, "Tache assignee"));
    }

    [HttpPost("tasks/{id}/start")]
    public async Task<IActionResult> StartTask(int id)
    {
        await _service.StartTaskAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Nettoyage demarre"));
    }

    [HttpPost("tasks/{id}/complete")]
    public async Task<IActionResult> CompleteTask(int id)
    {
        await _service.CompleteTaskAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Tache terminee — Chambre Libre"));
    }

    [HttpGet("maintenance")]
    public async Task<IActionResult> GetMaintenance()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllMaintenanceAsync()));

    [HttpPost("maintenance")]
    public async Task<IActionResult> CreateMaintenance([FromBody] CreateMaintenanceDto dto)
    {
        var req = await _service.CreateMaintenanceAsync(dto);
        return Ok(ApiResponse<object>.Ok(req, "Demande creee"));
    }

    [HttpPost("maintenance/{id}/close")]
    public async Task<IActionResult> CloseMaintenance(int id, [FromBody] string resolution)
    {
        await _service.CloseMaintenanceAsync(id, resolution);
        return Ok(ApiResponse<object>.Ok(new { id }, "Demande fermee — Chambre Libre"));
    }
}
