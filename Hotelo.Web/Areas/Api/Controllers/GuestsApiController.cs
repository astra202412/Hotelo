using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.Guest;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/guests")]
[ApiController]
[Authorize]
public class GuestsApiController : ControllerBase
{
    private readonly IGuestService _service;

    public GuestsApiController(IGuestService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllAsync()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var g = await _service.GetByIdAsync(id);
        return g == null
            ? NotFound(ApiResponse<object>.Fail("Client introuvable", 404))
            : Ok(ApiResponse<object>.Ok(g));
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string term)
        => Ok(ApiResponse<object>.Ok(await _service.SearchAsync(term)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGuestDto dto)
    {
        var g = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = g.Id },
            ApiResponse<object>.Ok(g, "Client cree"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGuestDto dto)
    {
        var g = await _service.UpdateAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(g, "Client mis a jour"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Client supprime"));
    }
}
