using Hotelo.Common.Helpers;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/backup")]
[ApiController]
[Authorize(Roles = "Admin")]
public class BackupApiController : ControllerBase
{
    private readonly IBackupService _service;
    public BackupApiController(IBackupService service) => _service = service;

    [HttpPost("create")]
    public async Task<IActionResult> Create()
    {
        try
        {
            var file = await _service.CreateBackupAsync();
            return Ok(ApiResponse<object>.Ok(new { file }, "Backup cree avec succes"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        var files = await _service.GetBackupListAsync();
        return Ok(ApiResponse<object>.Ok(files));
    }

    [HttpPost("restore")]
    public async Task<IActionResult> Restore([FromBody] string backupFile)
    {
        var success = await _service.RestoreBackupAsync(backupFile);
        return success
            ? Ok(ApiResponse<object>.Ok(new { }, "Restauration reussie"))
            : BadRequest(ApiResponse<object>.Fail("Echec de la restauration"));
    }
}
