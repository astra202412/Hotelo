using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/export")]
[ApiController]
[Authorize]
public class ExportApiController : ControllerBase
{
    private readonly IExportService _service;
    public ExportApiController(IExportService service) => _service = service;

    // PDF Facture
    [HttpGet("invoice/{id}/pdf")]
    public async Task<IActionResult> InvoicePdf(int id)
    {
        var bytes = await _service.ExportInvoicePdfAsync(id);
        return File(bytes, "text/html", $"Facture_{id}.html");
    }

    // Excel Reservations
    [HttpGet("reservations/excel")]
    public async Task<IActionResult> ReservationsExcel(
        DateTime? from = null, DateTime? to = null)
    {
        var dateFrom = from ?? DateTime.Today.AddMonths(-1);
        var dateTo = to ?? DateTime.Today.AddDays(1);
        var bytes = await _service.ExportReservationsExcelAsync(dateFrom, dateTo);
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Reservations_{dateFrom:yyyyMMdd}_{dateTo:yyyyMMdd}.xlsx");
    }

    // Excel Finances
    [HttpGet("finances/excel")]
    public async Task<IActionResult> FinancesExcel(
        DateTime? from = null, DateTime? to = null)
    {
        var dateFrom = from ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var dateTo = to ?? DateTime.Today.AddDays(1);
        var bytes = await _service.ExportFinancesExcelAsync(dateFrom, dateTo);
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Finances_{dateFrom:yyyyMMdd}_{dateTo:yyyyMMdd}.xlsx");
    }

    // Excel Chambres
    [HttpGet("rooms/excel")]
    public async Task<IActionResult> RoomsExcel()
    {
        var bytes = await _service.ExportRoomsExcelAsync();
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Chambres.xlsx");
    }

    // Excel Clients
    [HttpGet("guests/excel")]
    public async Task<IActionResult> GuestsExcel()
    {
        var bytes = await _service.ExportGuestsExcelAsync();
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Clients.xlsx");
    }
}
