using Hotelo.Core.Entities.Finances;

namespace Hotelo.Core.Interfaces.Repositories;

public interface IInvoiceRepository : IGenericRepository<Invoice>
{
    Task<IEnumerable<Invoice>> GetAllWithDetailsAsync();
    Task<Invoice?> GetWithDetailsAsync(int id);
    Task<Invoice?> GetByReservationAsync(int reservationId);
    Task<IEnumerable<Invoice>> GetUnpaidAsync();
    Task<decimal> GetRevenueTodayAsync();
    Task<decimal> GetRevenueMonthAsync();
    Task<List<(DateTime Date, decimal Revenue)>> GetRevenueLast7DaysAsync();
}
