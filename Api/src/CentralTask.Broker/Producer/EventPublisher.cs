using CentralTask.Broker.Connection;
using CentralTask.Broker.Interfaces;
using CentralTask.Core.DTO.Broker;
using CentralTask.Core.RepositoryBase;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CentralTask.Broker.Producer
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IEventConnection _connection;
        private readonly ILogger<EventPublisher> _logger;
        private const int MaxRetries = 3;

        public EventPublisher(IEventConnection connection, ILogger<EventPublisher> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<ValidateResult> PublishAsync(MessageRequestModel queueRequest)
        {
            var result = new ValidateResult();

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var channel = await _connection.CreateChannelAsync();

                    await channel.QueueDeclareAsync(
                        queue: queueRequest.QueueEvent.ToString(),
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(queueRequest));

                    var props = new BasicProperties
                    {
                        Persistent = true
                    };

                    await channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: queueRequest.QueueEvent.ToString(),
                        mandatory: false,
                        basicProperties: props,
                        body: body
                    );

                    _logger.LogInformation($"[RabbitMQ] Mensagem publicada na fila {queueRequest.QueueEvent}: {queueRequest.Message}");
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"[RabbitMQ] Tentativa {attempt}/{MaxRetries} falhou: {ex.Message}");

                    if (attempt == MaxRetries)
                    {
                        result.AddMessage($"Falha ao publicar mensagem após {MaxRetries} tentativas: {ex.Message}");
                        return result;
                    }

                    // Backoff exponencial
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                    _logger.LogInformation($"[RabbitMQ] Reagendando publicação em {delay.TotalSeconds} segundos...");
                    await Task.Delay(delay);
                }
            }

            return result;
        }
    }
}
