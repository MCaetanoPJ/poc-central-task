using CentralTask.Infra.Notifications.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace CentralTask.Infra.Notifications.Hubs
{
    public class UserConnectionService : IUserConnectionService
    {
        private readonly ConcurrentDictionary<string, string> _connections = new();

        public void AddConnection(string userId, string connectionId)
        {
            _connections[userId] = connectionId;
        }

        public void RemoveConnection(string userId)
        {
            _connections.TryRemove(userId, out _);
        }

        public string? GetConnectionId(string userId)
        {
            return _connections.TryGetValue(userId, out var connectionId) ? connectionId : null;
        }
    }
}
