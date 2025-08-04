using RabbitMQ.Client;

namespace CentralTask.Broker.Interfaces;

public interface IEventConnection
{
    Task<IChannel> CreateChannelAsync();
}