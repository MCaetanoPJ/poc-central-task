using CentralTask.Broker.Interfaces;
using CentralTask.Core.DTO.Broker;
using CentralTask.Core.Enums.Broker;
using CentralTask.Infra.Notifications.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CentralTask.Broker.Consumer
{
    public class EventConsumerBackgroundService : BackgroundService
    {
        private readonly IEventConsumer _consumer;
        private readonly ILogger<EventConsumerBackgroundService> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventConsumerBackgroundService(IEventConsumer consumer, 
            ILogger<EventConsumerBackgroundService> logger, 
            IHubContext<NotificationHub> hubContext)
        {
            _consumer = consumer;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var filas = new Dictionary<string, Func<MessageRequestModel, Task>>
            {
                [EnumMessageTypeEvent.TaskCreated.ToString()] = ExecuteQueueTaskCreatedAsync,
                [EnumMessageTypeEvent.TaskUpdated.ToString()] = ExecuteQueueTaskUpdatedAsync
            };

            var tasks = filas.Select(fila =>
                _consumer.ConsumeAsync(fila.Key, async message =>
                {
                    try
                    {
                        _logger.LogInformation($"Recebida mensagem da fila {fila.Key}: {message.Message}");
                        await fila.Value(message);
                        _logger.LogInformation($"Mensagem da fila {fila.Key} processada com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Erro ao processar mensagem da fila {fila.Key}: {ex.Message}");
                    }
                }, stoppingToken)
            );

            await Task.WhenAll(tasks);
        }
        private async Task ExecuteQueueTaskCreatedAsync(MessageRequestModel message)
        {
            if (message.Message is not null)
            {
                await _hubContext.Clients.User(message.UserId.ToString())
                    .SendAsync(EnumNotificationSignalR.NotificationTaskCreated.ToString(), new { Type = EnumMessageTypeEvent.TaskCreated.ToString(), Data = message });
            }
        }

        private async Task ExecuteQueueTaskUpdatedAsync(MessageRequestModel message)
        {
            if (message.Message is not null)
            {
                await _hubContext.Clients.User(message.UserId.ToString())
                    .SendAsync(EnumNotificationSignalR.NotificationTaskUpdated.ToString(), new { Type = EnumMessageTypeEvent.TaskUpdated.ToString(), Data = message });
            }
        }
    }
}
