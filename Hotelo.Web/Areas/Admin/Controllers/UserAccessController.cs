using Hotelo.Core.DTOs.Admin;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Directeur")]
public class UserAccessController : Controller
{
    private readonly IAdminService _adminService;
    public UserAccessController(IAdminService adminService) => _adminService = adminService;

    public async Task<IActionResult> Index()
    {
        var matrix = await _adminService.GetAccessMatrixAsync();
        return View(matrix);
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateUserAccessDto dto)
    {
        await _adminService.UpdateAccessAsync(dto);
        return Json(new { success = true });
    }
}