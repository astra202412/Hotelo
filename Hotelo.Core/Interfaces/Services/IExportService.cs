namespace Hotelo.Core.Interfaces.Services;

public interface IExportService
{
    Task<byte[]> ExportInvoicePdfAsync(int invoiceId);
    Task<byte[]> ExportReservationsExcelAsync(DateTime from, DateTime to);
    Task<byte[]> ExportFinancesExcelAsync(DateTime from, DateTime to);
    Task<byte[]> ExportRoomsExcelAsync();
    Task<byte[]> ExportGuestsExcelAsync();
}
