namespace CentralTask.Core.Notifications;

public interface INotifier
{
    IEnumerable<string> GetErrorList();
    IReadOnlyList<Notification> Notifications { get; }
    bool IsValid { get; }
    void Notify(params string[] notification);
}