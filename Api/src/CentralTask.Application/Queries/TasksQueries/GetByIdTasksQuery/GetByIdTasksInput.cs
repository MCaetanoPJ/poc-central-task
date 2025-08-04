
using CentralTask.Core.Mediator.Queries;

namespace CentralTask.Application.Queries.TasksQueries
{
    public class GetByIdTasksInput : QueryInput<QueryResult<GetByIdTasksItem>>
    {
        public Guid Id { get; set; }
    }
}