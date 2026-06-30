using Hotelo.Common.Enums;
using Hotelo.Core.DTOs.Reservation;
using Hotelo.Core.Entities.Reservation;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Core.Interfaces.Services;
using Hotelo.Infrastructure.Data;

namespace Hotelo.Infrastructure.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _resRepo;
    private readonly IRoomRepository _roomRepo;
    private readonly HoteloDbContext _context;
    private readonly INotificationService _notif;

    public ReservationService(IReservationRepository resRepo,
                               IRoomRepository roomRepo,
                               HoteloDbContext context,
                               INotificationService notif)
    {
        _resRepo = resRepo;
        _roomRepo = roomRepo;
        _context = context;
        _notif = notif;
    }

    private static string GetStatusLabel(ReservationStatus s) => s switch
    {
        ReservationStatus.EnAttente => "En Attente",
        ReservationStatus.Confirmee => "Confirmee",
        ReservationStatus.CheckedIn => "En Sejour",
        ReservationStatus.CheckedOut => "Termine",
        ReservationStatus.Annulee => "Annulee",
        ReservationStatus.NoShow => "No Show",
        _ => "Inconnu"
    };

    private static string GetStatusColor(ReservationStatus s) => s switch
    {
        ReservationStatus.EnAttente => "#FFC107",
        ReservationStatus.Confirmee => "#2196F3",
        ReservationStatus.CheckedIn => "#4CAF50",
        ReservationStatus.CheckedOut => "#9E9E9E",
        ReservationStatus.Annulee => "#F44336",
        ReservationStatus.NoShow => "#FF5722",
        _ => "#000000"
    };

    private static ReservationDto ToDto(Reservation r) => new()
    {
        Id = r.Id,
        ConfirmationCode = r.ConfirmationCode ?? "",
        GuestId = r.GuestId,
        GuestName = r.Guest?.FullName ?? "",
        GuestPhone = r.Guest?.Phone ?? "",
        RoomId = r.RoomId,
        RoomNumber = r.Room?.RoomNumber ?? "",
        RoomType = r.Room?.RoomType?.Name ?? "",
        CheckIn = r.CheckIn,
        CheckOut = r.CheckOut,
        Adults = r.Adults,
        Children = r.Children,
        Status = r.Status,
        StatusLabel = GetStatusLabel(r.Status),
        StatusColor = GetStatusColor(r.Status),
        RoomRate = r.RoomRate,
        PackagePrice = r.PackagePrice,
        Discount = r.Discount,
        TotalAmount = r.TotalAmount,
        PackageName = r.Package?.Name,
        SpecialRequests = r.SpecialRequests,
        CreatedAt = r.CreatedAt
    };

    public async Task<IEnumerable<ReservationDto>> GetAllAsync()
        => (await _resRepo.GetAllWithDetailsAsync()).Select(ToDto);

    public async Task<IEnumerable<ReservationDto>> GetTodayArrivalsAsync()
        => (await _resRepo.GetTodayArrivalsAsync()).Select(ToDto);

    public async Task<IEnumerable<ReservationDto>> GetTodayDeparturesAsync()
        => (await _resRepo.GetTodayDeparturesAsync()).Select(ToDto);

    public async Task<IEnumerable<ReservationDto>> GetByDateRangeAsync(DateTime from, DateTime to)
        => (await _resRepo.GetByDateRangeAsync(from, to)).Select(ToDto);

    public async Task<IEnumerable<ReservationDto>> GetByGuestAsync(int guestId)
        => (await _resRepo.GetByGuestAsync(guestId)).Select(ToDto);

    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        var r = await _resRepo.GetWithDetailsAsync(id);
        return r == null ? null : ToDto(r);
    }

    public async Task<ReservationDto> CreateAsync(CreateReservationDto dto)
    {
        if (!await IsRoomAvailableAsync(dto.RoomId, dto.CheckIn, dto.CheckOut))
            throw new InvalidOperationException("La chambre n est pas disponible pour ces dates.");

        var room = await _roomRepo.GetWithDetailsAsync(dto.RoomId)
            ?? throw new KeyNotFoundException("Chambre introuvable");
        var nights = (dto.CheckOut - dto.CheckIn).Days;
        var rate = room.RoomType?.BasePrice ?? 0;

        decimal packagePrice = 0;
        if (dto.PackageId.HasValue)
        {
            var pkg = await _context.Packages.FindAsync(dto.PackageId.Value);
            packagePrice = pkg?.AdditionalPrice ?? 0;
        }

        var total = ((rate + packagePrice) * nights) - dto.Discount;

        var reservation = new Reservation
        {
            GuestId = dto.GuestId,
            RoomId = dto.RoomId,
            PackageId = dto.PackageId,
            CheckIn = dto.CheckIn,
            CheckOut = dto.CheckOut,
            Adults = dto.Adults,
            Children = dto.Children,
            Status = ReservationStatus.Confirmee,
            RoomRate = rate,
            PackagePrice = packagePrice,
            Discount = dto.Discount,
            TotalAmount = total > 0 ? total : 0,
            SpecialRequests = dto.SpecialRequests,
            ConfirmationCode = $"HTL{DateTime.Now:yyyyMMddHHmmss}",
            CreatedAt = DateTime.UtcNow
        };

        await _resRepo.AddAsync(reservation);
        room.Status = RoomStatus.Reservee;
        await _roomRepo.UpdateAsync(room);

        return ToDto(await _resRepo.GetWithDetailsAsync(reservation.Id) ?? reservation);
    }

    public async Task<ReservationDto> UpdateAsync(int id, UpdateReservationDto dto)
    {
        var reservation = await _resRepo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Reservation {id} introuvable");

        if (!await IsRoomAvailableAsync(dto.RoomId, dto.CheckIn, dto.CheckOut, id))
            throw new InvalidOperationException("La chambre n est pas disponible.");

        var room = await _roomRepo.GetWithDetailsAsync(dto.RoomId);
        var nights = (dto.CheckOut - dto.CheckIn).Days;
        var rate = room?.RoomType?.BasePrice ?? reservation.RoomRate;
        var total = (rate * nights) - dto.Discount;

        reservation.GuestId = dto.GuestId;
        reservation.RoomId = dto.RoomId;
        reservation.PackageId = dto.PackageId;
        reservation.CheckIn = dto.CheckIn;
        reservation.CheckOut = dto.CheckOut;
        reservation.Adults = dto.Adults;
        reservation.Children = dto.Children;
        reservation.Discount = dto.Discount;
        reservation.TotalAmount = total > 0 ? total : 0;
        reservation.SpecialRequests = dto.SpecialRequests;

        await _resRepo.UpdateAsync(reservation);
        return ToDto(await _resRepo.GetWithDetailsAsync(id) ?? reservation);
    }

    public async Task CheckInAsync(int id)
    {
        var reservation = await _resRepo.GetWithDetailsAsync(id)
            ?? throw new KeyNotFoundException($"Reservation {id} introuvable");

        reservation.Status = ReservationStatus.CheckedIn;
        await _resRepo.UpdateAsync(reservation);

        var room = await _roomRepo.GetByIdAsync(reservation.RoomId);
        if (room != null)
        {
            room.Status = RoomStatus.Occupee;
            await _roomRepo.UpdateAsync(room);
        }

        // Notification SignalR
        await _notif.NotifyCheckInAsync(
            reservation.Guest?.FullName ?? "",
            reservation.Room?.RoomNumber ?? "");
        await _notif.NotifyRoomStatusChangedAsync(
            room?.Id ?? 0,
            room?.RoomNumber ?? "",
            "Occupee", "#F44336");
    }

    public async Task CheckOutAsync(int id)
    {
        var reservation = await _resRepo.GetWithDetailsAsync(id)
            ?? throw new KeyNotFoundException($"Reservation {id} introuvable");

        reservation.Status = ReservationStatus.CheckedOut;
        await _resRepo.UpdateAsync(reservation);

        var room = await _roomRepo.GetByIdAsync(reservation.RoomId);
        if (room != null)
        {
            room.Status = RoomStatus.ANettoyage;
            await _roomRepo.UpdateAsync(room);
        }

        // Notification SignalR
        await _notif.NotifyCheckOutAsync(
            reservation.Guest?.FullName ?? "",
            reservation.Room?.RoomNumber ?? "");
        await _notif.NotifyRoomStatusChangedAsync(
            room?.Id ?? 0,
            room?.RoomNumber ?? "",
            "A Nettoyer", "#FFC107");
    }

    public async Task CancelAsync(int id, string reason)
    {
        var reservation = await _resRepo.GetWithDetailsAsync(id)
            ?? throw new KeyNotFoundException($"Reservation {id} introuvable");

        reservation.Status = ReservationStatus.Annulee;
        reservation.SpecialRequests = $"[ANNULEE] {reason}";
        await _resRepo.UpdateAsync(reservation);

        var room = await _roomRepo.GetByIdAsync(reservation.RoomId);
        if (room != null && room.Status == RoomStatus.Reservee)
        {
            room.Status = RoomStatus.Libre;
            await _roomRepo.UpdateAsync(room);
        }
    }

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut, int? excludeId = null)
        => await _resRepo.IsRoomAvailableAsync(roomId, checkIn, checkOut, excludeId);
}
