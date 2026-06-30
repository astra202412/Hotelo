using Hotelo.Core.DTOs.Admin;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Directeur")]
public class EmployeesAdminController : Controller
{
    private readonly IAdminService _adminService;
    private readonly IHRService    _hrService;

    public EmployeesAdminController(IAdminService adminService, IHRService hrService)
    {
        _adminService = adminService;
        _hrService    = hrService;
    }

    // Liste employes avec statut compte utilisateur
    public async Task<IActionResult> Index()
    {
        var employees = await _adminService.GetEmployeesWithUserStatusAsync();
        return View(employees);
    }

    // Formulaire creation compte depuis employe
    public async Task<IActionResult> CreateUser(int id)
    {
        var employees = await _adminService.GetEmployeesWithUserStatusAsync();
        var employee  = employees.FirstOrDefault(e => e.EmployeeId == id);
        if (employee == null) return NotFound();
        if (employee.HasUser) return RedirectToAction("Index");
        var functions = await _adminService.GetFunctionsByDepartmentAsync(employee.DepartmentId);
        ViewBag.Functions = functions;
        ViewBag.Employee  = employee;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserFromEmployeeDto dto)
    {
        try
        {
            var userId = await _adminService.CreateUserFromEmployeeAsync(dto);
            return Json(new { success = true, userId });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    // Matrice droits utilisateur
    public async Task<IActionResult> ManageAccess(string id)
    {
        var matrix = await _adminService.GetUserAccessMatrixAsync(id);
        ViewBag.UserId = id;
        return View(matrix);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAccess([FromBody] UpdateUserAccessDto dto)
    {
        await _adminService.UpdateAccessAsync(dto);
        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> GetFunctions(int departmentId)
    {
        var functions = await _adminService.GetFunctionsByDepartmentAsync(departmentId);
        return Json(functions);
    }
}