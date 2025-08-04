using CentralTask.Broker.Connection;
using CentralTask.Broker.Interfaces;
using CentralTask.Core.DTO.Broker;
using CentralTask.Core.RepositoryBase;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CentralTask.Broker.Consumer
{
    public class EventConsumer : IEventConsumer
    {
        private readonly IEventConnection _connection;
        private readonly ILogger<EventConsumer> _logger;

        public EventConsumer(IEventConnection connection, ILogger<EventConsumer> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task ConsumeAsync(string queueName, Func<MessageRequestModel, Task> onMessageReceived, CancellationToken cancellationToken = default)
        {
            try
            {
                var channel = await _connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (sender, ea) =>
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    var body = ea.Body.ToArray();
                    var messageString = Encoding.UTF8.GetString(body);

                    try
                    {
                        var messageObj = JsonSerializer.Deserialize<MessageRequestModel>(messageString);

                        if (messageObj != null)
                        {
                            _logger.LogInformation($"Mensagem recebida da fila {queueName}: {messageObj.Message}");

                            await onMessageReceived(messageObj);

                            await channel.BasicAckAsync(ea.DeliveryTag, false);
                        }
                        else
                        {
                            _logger.LogWarning("Mensagem recebida não pôde ser desserializada.");
                            await channel.BasicNackAsync(ea.DeliveryTag, false, false); // rejeita sem requeue
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Erro ao processar mensagem: {ex.Message}");
                        await channel.BasicNackAsync(ea.DeliveryTag, false, true); // rejeita com requeue
                    }
                };

                await channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer);

                _logger.LogInformation($"Consumidor iniciado para a fila: {queueName}");

                // Mantém o método vivo até o cancelamento ser requisitado
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"Consumo da fila {queueName} cancelado.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao iniciar consumidor da fila {queueName}: {ex.Message}");
                throw;
            }
        }
    }

}
