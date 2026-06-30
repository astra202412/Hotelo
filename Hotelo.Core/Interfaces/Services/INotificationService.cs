using Hotelo.Core.DTOs.Notifications;

namespace Hotelo.Core.Interfaces.Services;

public interface INotificationService
{
    Task NotifyAllAsync(NotificationMessage message);
    Task NotifyRoomStatusChangedAsync(int roomId, string roomNumber, string status, string color);
    Task NotifyCheckInAsync(string guestName, string roomNumber);
    Task NotifyCheckOutAsync(string guestName, string roomNumber);
    Task NotifyMaintenanceAsync(string roomNumber, string description);
    Task NotifyHousekeepingAsync(string roomNumber, string taskStatus);
}
