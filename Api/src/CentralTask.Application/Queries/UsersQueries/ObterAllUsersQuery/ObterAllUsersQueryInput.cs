using CentralTask.Core.Mediator.Queries;

namespace CentralTask.Application.Queries.UserQueries.ObterTodosUserQuery;

public class GetAllUserQueryInput
    : QueryInput<QueryListResult<ObterTodosUsersQueryItem>>
{
}
