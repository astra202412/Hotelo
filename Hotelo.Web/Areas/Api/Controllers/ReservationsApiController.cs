using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.Reservation;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/reservations")]
[ApiController]
[Authorize]
public class ReservationsApiController : ControllerBase
{
    private readonly IReservationService _service;

    public ReservationsApiController(IReservationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllAsync()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var r = await _service.GetByIdAsync(id);
        return r == null
            ? NotFound(ApiResponse<object>.Fail("Reservation introuvable", 404))
            : Ok(ApiResponse<object>.Ok(r));
    }

    [HttpGet("arrivals/today")]
    public async Task<IActionResult> TodayArrivals()
        => Ok(ApiResponse<object>.Ok(await _service.GetTodayArrivalsAsync()));

    [HttpGet("departures/today")]
    public async Task<IActionResult> TodayDepartures()
        => Ok(ApiResponse<object>.Ok(await _service.GetTodayDeparturesAsync()));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
    {
        try
        {
            var r = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = r.Id },
                ApiResponse<object>.Ok(r, "Reservation creee"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReservationDto dto)
    {
        try
        {
            var r = await _service.UpdateAsync(id, dto);
            return Ok(ApiResponse<object>.Ok(r, "Reservation mise a jour"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPost("{id}/checkin")]
    public async Task<IActionResult> CheckIn(int id)
    {
        await _service.CheckInAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Check-in effectue"));
    }

    [HttpPost("{id}/checkout")]
    public async Task<IActionResult> CheckOut(int id)
    {
        await _service.CheckOutAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Check-out effectue"));
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id, [FromBody] string reason)
    {
        await _service.CancelAsync(id, reason);
        return Ok(ApiResponse<object>.Ok(new { id }, "Reservation annulee"));
    }

    [HttpGet("availability")]
    public async Task<IActionResult> CheckAvailability(int roomId, DateTime checkIn, DateTime checkOut)
    {
        var available = await _service.IsRoomAvailableAsync(roomId, checkIn, checkOut);
        return Ok(ApiResponse<object>.Ok(new { roomId, checkIn, checkOut, available }));
    }
}
