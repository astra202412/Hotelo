using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.HR;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/hr")]
[ApiController]
[Authorize]
public class HRApiController : ControllerBase
{
    private readonly IHRService _service;

    public HRApiController(IHRService service)
    {
        _service = service;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
        => Ok(ApiResponse<object>.Ok(await _service.GetDashboardAsync()));

    [HttpGet("employees")]
    public async Task<IActionResult> GetEmployees()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllEmployeesAsync()));

    [HttpGet("employees/{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        var emp = await _service.GetEmployeeByIdAsync(id);
        return emp == null
            ? NotFound(ApiResponse<object>.Fail("Employe introuvable", 404))
            : Ok(ApiResponse<object>.Ok(emp));
    }

    [HttpPost("employees")]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
    {
        var emp = await _service.CreateEmployeeAsync(dto);
        return Ok(ApiResponse<object>.Ok(emp, "Employe cree"));
    }

    [HttpPut("employees/{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
    {
        var emp = await _service.UpdateEmployeeAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(emp, "Employe mis a jour"));
    }

    [HttpDelete("employees/{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await _service.DeleteEmployeeAsync(id);
        return Ok(ApiResponse<object>.Ok(new { id }, "Employe desactive"));
    }

    [HttpGet("departments")]
    public async Task<IActionResult> GetDepartments()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllDepartmentsAsync()));

    [HttpPost("departments")]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto dto)
    {
        var dept = await _service.CreateDepartmentAsync(dto);
        return Ok(ApiResponse<object>.Ok(dept, "Departement cree"));
    }
}
