using Hotelo.Common.Helpers;
using Hotelo.Core.DTOs.Finances;
using Hotelo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelo.Web.Areas.Api.Controllers;

[Area("Api")]
[Route("api/v1/invoices")]
[ApiController]
[Authorize]
public class InvoicesApiController : ControllerBase
{
    private readonly IInvoiceService _service;

    public InvoicesApiController(IInvoiceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(ApiResponse<object>.Ok(await _service.GetAllAsync()));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var inv = await _service.GetByIdAsync(id);
        return inv == null
            ? NotFound(ApiResponse<object>.Fail("Facture introuvable", 404))
            : Ok(ApiResponse<object>.Ok(inv));
    }

    [HttpGet("reservation/{reservationId}")]
    public async Task<IActionResult> GetByReservation(int reservationId)
    {
        var inv = await _service.GetByReservationAsync(reservationId);
        return inv == null
            ? NotFound(ApiResponse<object>.Fail("Facture introuvable", 404))
            : Ok(ApiResponse<object>.Ok(inv));
    }

    [HttpGet("unpaid")]
    public async Task<IActionResult> GetUnpaid()
        => Ok(ApiResponse<object>.Ok(await _service.GetUnpaidAsync()));

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
        => Ok(ApiResponse<object>.Ok(await _service.GetDashboardAsync()));

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] CreateInvoiceDto dto)
    {
        try
        {
            var inv = await _service.GenerateAsync(dto);
            return Ok(ApiResponse<object>.Ok(inv, "Facture generee"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
        }
    }

    [HttpPost("payment")]
    public async Task<IActionResult> RegisterPayment([FromBody] RegisterPaymentDto dto)
    {
        try
        {
            var inv = await _service.RegisterPaymentAsync(dto);
            return Ok(ApiResponse<object>.Ok(inv, "Paiement enregistre"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message, 404));
        }
    }
}
