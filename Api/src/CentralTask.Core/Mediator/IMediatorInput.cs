using MediatR;

namespace CentralTask.Core.Mediator;

public interface IMediatorInput<out TMediatorResult>
    : IRequest<TMediatorResult> where TMediatorResult : IMediatorResult
{

}