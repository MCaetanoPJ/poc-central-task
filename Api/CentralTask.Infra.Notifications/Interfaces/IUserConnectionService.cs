namespace CentralTask.Infra.Notifications.Interfaces;

public interface IUserConnectionService
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string userId);
    string? GetConnectionId(string userId);
}