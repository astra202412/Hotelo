using Hotelo.Common.Enums;
using Hotelo.Core.DTOs.Finances;
using Hotelo.Core.Entities.Finances;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Core.Interfaces.Services;

namespace Hotelo.Infrastructure.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepo;
    private readonly IReservationRepository _resRepo;
    private const decimal TVA_RATE = 0.19m;

    public InvoiceService(IInvoiceRepository invoiceRepo,
                          IReservationRepository resRepo)
    {
        _invoiceRepo = invoiceRepo;
        _resRepo = resRepo;
    }

    private static InvoiceDto ToDto(Invoice i) => new()
    {
        Id = i.Id,
        InvoiceNumber = i.InvoiceNumber,
        ReservationId = i.ReservationId,
        ConfirmationCode = i.Reservation?.ConfirmationCode ?? "",
        GuestName = i.Reservation?.Guest?.FullName ?? "",
        RoomNumber = i.Reservation?.Room?.RoomNumber ?? "",
        RoomType = i.Reservation?.Room?.RoomType?.Name ?? "",
        CheckIn = i.Reservation?.CheckIn ?? DateTime.MinValue,
        CheckOut = i.Reservation?.CheckOut ?? DateTime.MinValue,
        Nights = i.Reservation != null ? (i.Reservation.CheckOut - i.Reservation.CheckIn).Days : 0,
        RoomRate = i.Reservation?.RoomRate ?? 0,
        PackagePrice = i.Reservation?.PackagePrice ?? 0,
        TotalHT = i.TotalHT,
        TVARate = i.TVARate,
        TVAAmount = i.TVAAmount,
        Discount = i.Discount,
        TotalTTC = i.TotalTTC,
        IsPaid = i.IsPaid,
        PaidAt = i.PaidAt,
        Notes = i.Notes,
        CreatedAt = i.CreatedAt,
        Payments = i.Payments?.Select(p => new PaymentDto
        {
            Id = p.Id,
            Amount = p.Amount,
            Method = p.Method.ToString(),
            Reference = p.Reference,
            PaidAt = p.PaidAt,
            Notes = p.Notes
        }).ToList() ?? new()
    };

    public async Task<IEnumerable<InvoiceDto>> GetAllAsync()
        => (await _invoiceRepo.GetAllWithDetailsAsync()).Select(ToDto);

    public async Task<InvoiceDto?> GetByIdAsync(int id)
    {
        var i = await _invoiceRepo.GetWithDetailsAsync(id);
        return i == null ? null : ToDto(i);
    }

    public async Task<InvoiceDto?> GetByReservationAsync(int reservationId)
    {
        var i = await _invoiceRepo.GetByReservationAsync(reservationId);
        return i == null ? null : ToDto(i);
    }

    public async Task<IEnumerable<InvoiceDto>> GetUnpaidAsync()
        => (await _invoiceRepo.GetUnpaidAsync()).Select(ToDto);

    public async Task<InvoiceDto> GenerateAsync(CreateInvoiceDto dto)
    {
        // Verifier si facture existe deja
        var existing = await _invoiceRepo.GetByReservationAsync(dto.ReservationId);
        if (existing != null)
            return ToDto(existing);

        var reservation = await _resRepo.GetWithDetailsAsync(dto.ReservationId)
            ?? throw new KeyNotFoundException("Reservation introuvable");

        var nights = (reservation.CheckOut - reservation.CheckIn).Days;
        var totalHT = (reservation.RoomRate + reservation.PackagePrice) * nights - dto.Discount;
        if (totalHT < 0) totalHT = 0;
        var tvaAmt = Math.Round(totalHT * TVA_RATE, 2);
        var totalTTC = totalHT + tvaAmt;

        var invoice = new Invoice
        {
            ReservationId = dto.ReservationId,
            InvoiceNumber = $"FAC{DateTime.Now:yyyyMMddHHmmss}",
            TotalHT = totalHT,
            TVARate = TVA_RATE,
            TVAAmount = tvaAmt,
            TotalTTC = totalTTC,
            Discount = dto.Discount,
            IsPaid = false,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _invoiceRepo.AddAsync(invoice);
        return ToDto(await _invoiceRepo.GetWithDetailsAsync(invoice.Id) ?? invoice);
    }

    public async Task<InvoiceDto> RegisterPaymentAsync(RegisterPaymentDto dto)
    {
        var invoice = await _invoiceRepo.GetWithDetailsAsync(dto.InvoiceId)
            ?? throw new KeyNotFoundException("Facture introuvable");

        if (!Enum.TryParse<PaymentMethod>(dto.Method, out var method))
            method = PaymentMethod.Especes;

        var payment = new Payment
        {
            InvoiceId = dto.InvoiceId,
            Amount = dto.Amount,
            Method = method,
            Reference = dto.Reference,
            PaidAt = DateTime.UtcNow,
            Notes = dto.Notes
        };

        invoice.Payments.Add(payment);

        var totalPaid = invoice.Payments.Sum(p => p.Amount);
        if (totalPaid >= invoice.TotalTTC)
        {
            invoice.IsPaid = true;
            invoice.PaidAt = DateTime.UtcNow;
        }

        await _invoiceRepo.UpdateAsync(invoice);
        return ToDto(await _invoiceRepo.GetWithDetailsAsync(invoice.Id) ?? invoice);
    }

    public async Task<FinancesDashboardDto> GetDashboardAsync()
    {
        var all = (await _invoiceRepo.GetAllWithDetailsAsync()).ToList();
        var last7 = await _invoiceRepo.GetRevenueLast7DaysAsync();

        return new FinancesDashboardDto
        {
            TotalRevenueToday = await _invoiceRepo.GetRevenueTodayAsync(),
            TotalRevenueMonth = await _invoiceRepo.GetRevenueMonthAsync(),
            InvoicesCount = all.Count,
            PaidInvoices = all.Count(i => i.IsPaid),
            UnpaidInvoices = all.Count(i => !i.IsPaid),
            PendingAmount = all.Where(i => !i.IsPaid).Sum(i => i.TotalTTC),
            RevenueLast7Days = Enumerable.Range(0, 7)
                .Select(d => DateTime.Today.AddDays(-6 + d))
                .Select(date => new RevenueDayDto
                {
                    Day = date.ToString("dd/MM"),
                    Revenue = last7.FirstOrDefault(x => x.Date == date).Revenue
                }).ToList()
        };
    }
}
