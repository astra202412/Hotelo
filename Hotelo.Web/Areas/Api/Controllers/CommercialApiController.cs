using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.Commercial;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/commercial")]
[ApiController]
[Authorize]
public class CommercialApiController : ControllerBase
{
    private readonly ICommercialService _service;
    public CommercialApiController(ICommercialService service) => _service = service;

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
        => Ok(ApiResponse<object>.Ok(await _service.GetDashboardAsync()));

    [HttpGet("companies")]
    public async Task<IActionResult> GetCompanies()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllCompaniesAsync()));

    [HttpGet("companies/{id}")]
    public async Task<IActionResult> GetCompany(int id)
    {
        var c = await _service.GetCompanyByIdAsync(id);
        return c == null ? NotFound(ApiResponse<object>.Fail("Introuvable", 404))
                         : Ok(ApiResponse<object>.Ok(c));
    }

    [HttpPost("companies")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto dto)
        => Ok(ApiResponse<object>.Ok(await _service.CreateCompanyAsync(dto), "Entreprise creee"));

    [HttpDelete("companies/{id}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        await _service.DeleteCompanyAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Entreprise desactivee"));
    }

    [HttpGet("contracts")]
    public async Task<IActionResult> GetContracts()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllContractsAsync()));

    [HttpPost("contracts")]
    public async Task<IActionResult> CreateContract([FromBody] CreateContractDto dto)
        => Ok(ApiResponse<object>.Ok(await _service.CreateContractAsync(dto), "Contrat cree"));
}
