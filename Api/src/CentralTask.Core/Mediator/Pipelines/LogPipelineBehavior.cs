using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CentralTask.Core.Mediator.Pipelines;

public class LogPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMediatorInput<TResponse>
    where TResponse : IMediatorResult
{
    private readonly ILogger<LogPipelineBehavior<TRequest, TResponse>> _logger;

    public LogPipelineBehavior(ILogger<LogPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogDebug("{RequestType} - Entering handler... {Request}", typeof(TRequest).Name, JsonConvert.SerializeObject(request));

        var result = await next();

        _logger.LogDebug("{RequestType} - Leaving handler!", typeof(TRequest).Name);

        return result;
    }
}