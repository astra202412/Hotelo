using Hotelo.Common.Enums;

namespace Hotelo.Core.DTOs.Reservation;

public class ReservationDto
{
    public int Id { get; set; }
    public string ConfirmationCode { get; set; } = string.Empty;
    public int GuestId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string GuestPhone { get; set; } = string.Empty;
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Nights => (CheckOut - CheckIn).Days;
    public int Adults { get; set; }
    public int Children { get; set; }
    public ReservationStatus Status { get; set; }
    public string StatusLabel { get; set; } = string.Empty;
    public string StatusColor { get; set; } = string.Empty;
    public decimal RoomRate { get; set; }
    public decimal PackagePrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? PackageName { get; set; }
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateReservationDto
{
    public int GuestId { get; set; }
    public int RoomId { get; set; }
    public int? PackageId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Adults { get; set; } = 1;
    public int Children { get; set; } = 0;
    public decimal Discount { get; set; } = 0;
    public string? SpecialRequests { get; set; }
}

public class UpdateReservationDto : CreateReservationDto { }
