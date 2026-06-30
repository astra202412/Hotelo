using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.Admin;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/admin")]
[ApiController]
[Authorize(Roles = "Admin,Directeur")]
public class AdminApiController : ControllerBase
{
    private readonly IAdminService _service;
    public AdminApiController(IAdminService service) => _service = service;

    // Fonctions
    [HttpGet("functions")]
    public async Task<IActionResult> GetFunctions()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllFunctionsAsync()));

    [HttpPost("functions")]
    public async Task<IActionResult> CreateFunction([FromBody] CreateJobFunctionDto dto)
        => Ok(ApiResponse<object>.Ok(await _service.CreateFunctionAsync(dto), "Fonction creee"));

    [HttpDelete("functions/{id}")]
    public async Task<IActionResult> DeleteFunction(int id)
    {
        await _service.DeleteFunctionAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Fonction desactivee"));
    }

    // Modules
    [HttpGet("modules")]
    public async Task<IActionResult> GetModules()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllModulesAsync()));

    // Matrice acces
    [HttpGet("access-matrix")]
    public async Task<IActionResult> GetMatrix()
        => Ok(ApiResponse<object>.Ok(await _service.GetAccessMatrixAsync()));

    [HttpPost("access")]
    public async Task<IActionResult> UpdateAccess([FromBody] UpdateUserAccessDto dto)
    {
        await _service.UpdateAccessAsync(dto);
        return Ok(ApiResponse<object>.Ok(new { }, "Acces mis a jour"));
    }

    // Affectation utilisateurs
    [HttpGet("user-functions")]
    public async Task<IActionResult> GetUserFunctions()
        => Ok(ApiResponse<object>.Ok(await _service.GetUserFunctionsAsync()));

    [HttpPost("user-functions")]
    public async Task<IActionResult> AssignFunction([FromBody] AssignFunctionDto dto)
    {
        await _service.AssignFunctionToUserAsync(dto.UserId, dto.FunctionId);
        return Ok(ApiResponse<object>.Ok(new { }, "Fonction assignee"));
    }

    [HttpDelete("user-functions/{userId}/{functionId}")]
    public async Task<IActionResult> RemoveFunction(string userId, int functionId)
    {
        await _service.RemoveFunctionFromUserAsync(userId, functionId);
        return Ok(ApiResponse<object>.Ok(new { }, "Fonction retiree"));
    }
}

public class AssignFunctionDto
{
    public string UserId     { get; set; } = string.Empty;
    public int    FunctionId { get; set; }
}
