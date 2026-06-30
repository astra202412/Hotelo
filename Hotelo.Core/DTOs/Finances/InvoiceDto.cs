namespace Hotelo.Core.DTOs.Finances;

public class InvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int ReservationId { get; set; }
    public string ConfirmationCode { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Nights { get; set; }
    public decimal RoomRate { get; set; }
    public decimal PackagePrice { get; set; }
    public decimal TotalHT { get; set; }
    public decimal TVARate { get; set; }
    public decimal TVAAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalTTC { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<PaymentDto> Payments { get; set; } = new();
}

public class CreateInvoiceDto
{
    public int ReservationId { get; set; }
    public decimal Discount { get; set; } = 0;
    public string? Notes { get; set; }
}

public class RegisterPaymentDto
{
    public int InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = "Especes";
    public string? Reference { get; set; }
    public string? Notes { get; set; }
}

public class PaymentDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public DateTime PaidAt { get; set; }
    public string? Notes { get; set; }
}

public class FinancesDashboardDto
{
    public decimal TotalRevenueMonth { get; set; }
    public decimal TotalRevenueToday { get; set; }
    public int InvoicesCount { get; set; }
    public int PaidInvoices { get; set; }
    public int UnpaidInvoices { get; set; }
    public decimal PendingAmount { get; set; }
    public List<RevenueDayDto> RevenueLast7Days { get; set; } = new();
}

public class RevenueDayDto
{
    public string Day { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}
