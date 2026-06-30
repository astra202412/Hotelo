using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Hotelo.Web.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public async Task SendNotification(string userId, string title, string message)
        => await Clients.User(userId).SendAsync("ReceiveNotification", title, message);

    public async Task BroadcastRoomStatus(int roomId, string status)
        => await Clients.All.SendAsync("RoomStatusUpdated", roomId, status);
}