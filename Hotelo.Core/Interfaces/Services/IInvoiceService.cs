using Hotelo.Core.DTOs.Finances;

namespace Hotelo.Core.Interfaces.Services;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceDto>> GetAllAsync();
    Task<InvoiceDto?> GetByIdAsync(int id);
    Task<InvoiceDto?> GetByReservationAsync(int reservationId);
    Task<InvoiceDto> GenerateAsync(CreateInvoiceDto dto);
    Task<InvoiceDto> RegisterPaymentAsync(RegisterPaymentDto dto);
    Task<FinancesDashboardDto> GetDashboardAsync();
    Task<IEnumerable<InvoiceDto>> GetUnpaidAsync();
}
