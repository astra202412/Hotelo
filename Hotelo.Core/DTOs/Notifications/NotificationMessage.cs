namespace Hotelo.Core.DTOs.Notifications;

public class NotificationMessage
{
    public string Type { get; set; } = "Info"; // Info, Success, Warning, Error
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Data { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
