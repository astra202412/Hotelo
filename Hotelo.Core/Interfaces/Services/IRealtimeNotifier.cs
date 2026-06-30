namespace Hotelo.Core.Interfaces.Services;

public interface IRealtimeNotifier
{
    Task SendToAllAsync(string method, object data);
}
