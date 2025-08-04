using CentralTask.Core.Notifications;
using MediatR;

namespace CentralTask.Core.Mediator.Pipelines;

public class NotificationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMediatorInput<TResponse>
    where TResponse : IMediatorResult
{
    private readonly INotifier _notifier;

    public NotificationPipelineBehavior(INotifier notifier)
    {
        _notifier = notifier;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (!_notifier.IsValid)
        {
            foreach (var notification in _notifier.Notifications)
            {
                if (response.Errors.Contains(notification.Message)) continue;
                response.AddError(notification.Message);
            }
        }

        return response;
    }
}