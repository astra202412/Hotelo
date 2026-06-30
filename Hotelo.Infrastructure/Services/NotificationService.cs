using Hotelo.Core.DTOs.Notifications;
using Hotelo.Core.Interfaces.Services;

namespace Hotelo.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IRealtimeNotifier _notifier;

    public NotificationService(IRealtimeNotifier notifier)
    {
        _notifier = notifier;
    }

    public async Task NotifyAllAsync(NotificationMessage message)
        => await _notifier.SendToAllAsync("ReceiveNotification", message);

    public async Task NotifyRoomStatusChangedAsync(int roomId, string roomNumber, string status, string color)
        => await _notifier.SendToAllAsync("RoomStatusUpdated", new
        {
            roomId,
            roomNumber,
            status,
            color,
            timestamp = DateTime.UtcNow
        });

    public async Task NotifyCheckInAsync(string guestName, string roomNumber)
        => await NotifyAllAsync(new NotificationMessage
        {
            Type = "Success",
            Title = "Check-In Effectue",
            Message = $"{guestName} vient d arriver — Chambre {roomNumber}"
        });

    public async Task NotifyCheckOutAsync(string guestName, string roomNumber)
        => await NotifyAllAsync(new NotificationMessage
        {
            Type = "Info",
            Title = "Check-Out Effectue",
            Message = $"{guestName} vient de quitter — Chambre {roomNumber}"
        });

    public async Task NotifyMaintenanceAsync(string roomNumber, string description)
        => await NotifyAllAsync(new NotificationMessage
        {
            Type = "Warning",
            Title = "Maintenance Requise",
            Message = $"Chambre {roomNumber} : {description}"
        });

    public async Task NotifyHousekeepingAsync(string roomNumber, string taskStatus)
        => await NotifyAllAsync(new NotificationMessage
        {
            Type = "Info",
            Title = "Housekeeping",
            Message = $"Chambre {roomNumber} : {taskStatus}"
        });
}
