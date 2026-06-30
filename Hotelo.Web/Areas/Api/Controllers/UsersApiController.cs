using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.Identity;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/users")]
[ApiController]
[Authorize]
public class UsersApiController : ControllerBase
{
    private readonly IUserService _service;
    public UsersApiController(IUserService service) => _service = service;

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
        => Ok(ApiResponse<object>.Ok(await _service.GetDashboardAsync()));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllUsersAsync()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var u = await _service.GetByIdAsync(id);
        return u == null ? NotFound(ApiResponse<object>.Fail("Introuvable", 404))
                         : Ok(ApiResponse<object>.Ok(u));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        try
        {
            var u = await _service.CreateUserAsync(dto);
            return Ok(ApiResponse<object>.Ok(u, "Utilisateur cree"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
    {
        var u = await _service.UpdateUserAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(u, "Utilisateur mis a jour"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteUserAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Utilisateur desactive"));
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        try
        {
            await _service.ChangePasswordAsync(dto);
            return Ok(ApiResponse<object>.Ok(new { }, "Mot de passe modifie"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPost("{id}/role")]
    public async Task<IActionResult> AssignRole(string id, [FromBody] string role)
    {
        await _service.AssignRoleAsync(id, role);
        return Ok(ApiResponse<object>.Ok(new { id, role }, "Role assigne"));
    }
}
