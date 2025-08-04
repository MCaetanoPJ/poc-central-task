using CentralTask.Infra.Notifications.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace CentralTask.Infra.Notifications.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IUserConnectionService _userConnectionService;
        private static readonly ConcurrentDictionary<Guid, HashSet<string>> _userConnections = new();

        public NotificationHub(IUserConnectionService userConnectionService)
        {
            _userConnectionService = userConnectionService;
        }
        public override async Task OnConnectedAsync()
        {
            Guid? userId = GetUserIdFromContext();

            if (userId == null)
            {
                Context.Abort();
                return;
            }

            _userConnections.AddOrUpdate(
                userId.Value,
                _ => new HashSet<string> { Context.ConnectionId },
                (_, connections) =>
                {
                    connections.Add(Context.ConnectionId);
                    return connections;
                });

            await Groups.AddToGroupAsync(Context.ConnectionId, userId.Value.ToString());

            _userConnectionService.AddConnection(userId.Value.ToString(), Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        private Guid? GetUserIdFromContext()
        {
            if (Guid.TryParse(Context.UserIdentifier, out var userId))
            {
                return userId;
            }

            var claim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(claim, out userId))
            {
                return userId;
            }

            return null;
        }
    }
}