namespace CentralTask.Core.Notifications;

public class Notification
{
    public Notification(string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        Message = message;
    }

    public string Message { get; }

    public static implicit operator Notification(string message) => new(message);
}