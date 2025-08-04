using CentralTask.Core.DTO.Broker;
using CentralTask.Core.RepositoryBase;

namespace CentralTask.Broker.Interfaces;

public interface IEventPublisher
{
    Task<ValidateResult> PublishAsync(MessageRequestModel queueRequest);
}