using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BackupController : Controller
{
    private readonly IBackupService _backupService;
    public BackupController(IBackupService backupService)
        => _backupService = backupService;

    public async Task<IActionResult> Index()
    {
        var files = await _backupService.GetBackupListAsync();
        return View(files);
    }
}
