using Hotelo.Core.Interfaces.Services;
using Hotelo.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Hotelo.Web.Services;

public class SignalRNotifier : IRealtimeNotifier
{
    private readonly IHubContext<NotificationHub> _hub;
    public SignalRNotifier(IHubContext<NotificationHub> hub) => _hub = hub;

    public async Task SendToAllAsync(string method, object data)
        => await _hub.Clients.All.SendAsync(method, data);
}
