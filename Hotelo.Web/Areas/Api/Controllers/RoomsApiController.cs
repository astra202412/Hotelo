using Hotelo.Common.Enums;
using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.Room;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/rooms")]
[ApiController]
[Authorize]
public class RoomsApiController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsApiController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    // GET api/v1/rooms
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _roomService.GetAllAsync();
        return Ok(ApiResponse<object>.Ok(rooms));
    }

    // GET api/v1/rooms/plan
    [HttpGet("plan")]
    public async Task<IActionResult> GetPlan()
    {
        var plan = await _roomService.GetFloorPlanAsync();
        return Ok(ApiResponse<object>.Ok(plan));
    }

    // GET api/v1/rooms/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var room = await _roomService.GetByIdAsync(id);
        if (room == null) return NotFound(ApiResponse<object>.Fail("Chambre introuvable", 404));
        return Ok(ApiResponse<object>.Ok(room));
    }

    // GET api/v1/rooms/{id}/availability
    [HttpGet("{id}/availability")]
    public async Task<IActionResult> GetAvailability(int id)
    {
        var room = await _roomService.GetByIdAsync(id);
        if (room == null) return NotFound(ApiResponse<object>.Fail("Chambre introuvable", 404));
        return Ok(ApiResponse<object>.Ok(new
        {
            roomId = id,
            available = room.Status == RoomStatus.Libre,
            status = room.StatusLabel
        }));
    }

    // POST api/v1/rooms
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var room = await _roomService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = room.Id },
            ApiResponse<object>.Ok(room, "Chambre creee"));
    }

    // PUT api/v1/rooms/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoomDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var room = await _roomService.UpdateAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(room, "Chambre mise a jour"));
    }

    // PATCH api/v1/rooms/{id}/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateRoomStatusDto dto)
    {
        await _roomService.UpdateStatusAsync(id, dto.Status);
        return Ok(ApiResponse<object>.Ok(new { id, status = dto.Status }, "Statut mis a jour"));
    }

    // DELETE api/v1/rooms/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _roomService.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Chambre desactivee"));
    }

    // GET api/v1/rooms/dashboard
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = await _roomService.GetDashboardAsync();
        return Ok(ApiResponse<object>.Ok(dashboard));
    }
}
