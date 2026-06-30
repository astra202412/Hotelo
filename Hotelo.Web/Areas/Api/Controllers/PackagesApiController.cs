using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.Package;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/packages")]
[ApiController]
[Authorize]
public class PackagesApiController : ControllerBase
{
    private readonly IPackageService _service;
    public PackagesApiController(IPackageService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllAsync()));

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive()
        => Ok(ApiResponse<object>.Ok(await _service.GetActiveAsync()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _service.GetByIdAsync(id);
        return p == null
            ? NotFound(ApiResponse<object>.Fail("Package introuvable", 404))
            : Ok(ApiResponse<object>.Ok(p));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePackageDto dto)
    {
        var p = await _service.CreateAsync(dto);
        return Ok(ApiResponse<object>.Ok(p, "Package cree"));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePackageDto dto)
    {
        var p = await _service.UpdateAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(p, "Package mis a jour"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Package desactive"));
    }
}
