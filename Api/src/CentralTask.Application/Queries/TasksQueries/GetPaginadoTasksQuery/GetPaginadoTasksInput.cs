
using CentralTask.Core.Mediator.Queries;

namespace CentralTask.Application.Queries.TasksQueries
{
    public class GetPaginadoTasksInput : QueryPaginatedInput<QueryPaginatedResult<GetPaginadoTasksItem>>
    {
        public string? Nome { get; set; }
    }
}