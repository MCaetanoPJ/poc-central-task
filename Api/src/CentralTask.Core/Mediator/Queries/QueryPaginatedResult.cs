namespace CentralTask.Core.Mediator.Queries;

public class QueryPaginatedResult<TItem> : QueryListResult<TItem>
{
    public QueryPagination Pagination { get; set; } = new();
}