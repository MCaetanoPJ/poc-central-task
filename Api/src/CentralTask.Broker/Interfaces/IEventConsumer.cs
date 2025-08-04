using CentralTask.Core.DTO.Broker;
using CentralTask.Core.RepositoryBase;

namespace CentralTask.Broker.Interfaces;

public interface IEventConsumer
{
    Task ConsumeAsync(string queueName, Func<MessageRequestModel, Task> onMessageReceived, CancellationToken cancellationToken = default);
}