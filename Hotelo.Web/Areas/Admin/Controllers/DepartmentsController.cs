using Hotelo.Core.DTOs.Admin;
using Hotelo.Core.DTOs.HR;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Directeur")]
public class DepartmentsController : Controller
{
    private readonly IHRService    _hrService;
    private readonly IAdminService _adminService;

    public DepartmentsController(IHRService hrService, IAdminService adminService)
    {
        _hrService    = hrService;
        _adminService = adminService;
    }

    // GET /Admin/Departments
    public async Task<IActionResult> Index()
    {
        var depts     = await _hrService.GetAllDepartmentsAsync();
        var functions = await _adminService.GetAllFunctionsAsync();
        ViewBag.Functions = functions;
        return View(depts);
    }

    // POST /Admin/Departments/Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
    {
        try
        {
            var result = await _hrService.CreateDepartmentAsync(dto);
            return Json(new { success = true, data = result });
        }
        catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
    }

    // GET /Admin/Departments/GetById/5
    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var dept = await _hrService.GetDepartmentByIdAsync(id);
        if (dept == null) return Json(new { success = false });
        return Json(new { success = true, data = dept });
    }

    // POST /Admin/Departments/Update/5
    [HttpPost]
    public async Task<IActionResult> Update(int id, [FromBody] CreateDepartmentDto dto)
    {
        try
        {
            var result = await _hrService.UpdateDepartmentAsync(id, dto);
            return Json(new { success = true, data = result });
        }
        catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
    }

    // POST /Admin/Departments/Delete/5
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _hrService.DeleteDepartmentAsync(id);
            return Json(new { success = true });
        }
        catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
    }

    // GET /Admin/Departments/Functions?departmentId=X  (AJAX)
    [HttpGet]
    public async Task<IActionResult> Functions(int departmentId)
    {
        var functions = await _adminService.GetFunctionsByDepartmentAsync(departmentId);
        return Json(functions);
    }

    // POST /Admin/Departments/CreateFunction
    [HttpPost]
    public async Task<IActionResult> CreateFunction([FromBody] CreateJobFunctionDto dto)
    {
        var result = await _adminService.CreateFunctionAsync(dto);
        return Json(new { success = true, data = result });
    }

    // POST /Admin/Departments/DeleteFunction/5
    [HttpPost]
    public async Task<IActionResult> DeleteFunction(int id)
    {
        await _adminService.DeleteFunctionAsync(id);
        return Json(new { success = true });
    }
}