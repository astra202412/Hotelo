using Hotelo.Common.Enums;
using Hotelo.Common.Helpers;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.FrontOffice.Controllers;

[Area("FrontOffice")]
[Authorize]
public class HomeController : Controller
{
    private readonly IRoomService _roomService;

    public HomeController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    // Plan interactif des chambres
    public async Task<IActionResult> Index()
    {
        var plan = await _roomService.GetFloorPlanAsync();
        return View(plan);
    }

    // Liste des chambres
    public async Task<IActionResult> List()
    {
        var rooms = await _roomService.GetAllAsync();
        return View(rooms);
    }

    // Dashboard
    public async Task<IActionResult> Dashboard()
    {
        var dashboard = await _roomService.GetDashboardAsync();
        return View(dashboard);
    }

    // Formulaire création
    public IActionResult Create()
    {
        return View();
    }

    // Formulaire édition
    public async Task<IActionResult> Edit(int id)
    {
        var room = await _roomService.GetByIdAsync(id);
        if (room == null) return NotFound();
        return View(room);
    }

    // API — Mettre à jour statut (appellé par JS)
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, RoomStatus status)
    {
        try
        {
            await _roomService.UpdateStatusAsync(id, status);
            return Ok(ApiResponse<object>.Ok(new { id, status }, "Statut mis a jour"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    // API — Détails chambre (JSON pour modal)
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var room = await _roomService.GetByIdAsync(id);
        if (room == null) return NotFound();
        return Json(room);
    }
}
