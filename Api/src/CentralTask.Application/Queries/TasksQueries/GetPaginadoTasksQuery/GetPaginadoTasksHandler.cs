using CentralTask.Core.Extensions;
using CentralTask.Core.Mediator.Queries;
using CentralTask.Domain.Interfaces.Repositories;

namespace CentralTask.Application.Queries.TasksQueries
{
    public class GetPaginadoTasksHandler : IQueryHandler<GetPaginadoTasksInput, QueryPaginatedResult<GetPaginadoTasksItem>>
    {
        private readonly ITasksRepository _repository;

        public GetPaginadoTasksHandler(ITasksRepository repository)
        {
            _repository = repository;
        }

        public async Task<QueryPaginatedResult<GetPaginadoTasksItem>> Handle(GetPaginadoTasksInput request, CancellationToken cancellationToken)
        {
            var query = _repository.GetAsNoTracking();

            var (result, pagination) = await query
                .Select(x => new GetPaginadoTasksItem
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    DueDate = x.DueDate,
                    UserId = x.UserId,
                    Status = (int)x.Status
                })
                .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);

            return new QueryPaginatedResult<GetPaginadoTasksItem>
            {
                Pagination = pagination,
                Result = result
            };
        }
    }
}