
using CentralTask.Core.Mediator.Queries;
using CentralTask.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CentralTask.Application.Queries.TasksQueries
{
    public class GetByIdTasksHandler : IQueryHandler<GetByIdTasksInput, QueryResult<GetByIdTasksItem>>
    {
        private readonly ITasksRepository _repository;

        public GetByIdTasksHandler(ITasksRepository repository)
        {
            _repository = repository;
        }

        public async Task<QueryResult<GetByIdTasksItem>> Handle(GetByIdTasksInput request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (result == null)
                return null;

            return new QueryResult<GetByIdTasksItem>
            {
                Result = new GetByIdTasksItem
                {
                    Title = result.Title,
                    Description = result.Description,
                    DueDate = result.DueDate,
                    UserId = result.UserId,
                    CreatedAt = result.CreatedAt,
                    Status = (int)result.Status
                }
            };
        }
    }
}