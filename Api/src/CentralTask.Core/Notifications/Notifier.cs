namespace CentralTask.Core.Notifications;

public class Notifier : INotifier
{
    private readonly List<Notification> _notifications = new();

    public IReadOnlyList<Notification> Notifications => _notifications;

    public bool IsValid => Notifications.Count == 0;

    public void Notify(params string[] notifications)
    {
        _notifications.AddRange(notifications.Select(x => new Notification(x)));
    }

    public IEnumerable<string> GetErrorList()
    {
        return Notifications.Select(x => x.Message);
    }
}