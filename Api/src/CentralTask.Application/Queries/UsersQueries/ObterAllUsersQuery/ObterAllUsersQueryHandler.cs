using AutoMapper;
using CentralTask.Core.Mediator.Queries;
using CentralTask.Domain.Interfaces.Repositories;

namespace CentralTask.Application.Queries.UserQueries.ObterTodosUserQuery;

public class ObterTodosUsersQueryHandler
    : IQueryHandler<GetAllUserQueryInput,
    QueryListResult<ObterTodosUsersQueryItem>>
{
    private readonly IUserRepository _UserRepository;
    private readonly IMapper _mapper;
    public ObterTodosUsersQueryHandler(
        IUserRepository UserRepository,
        IMapper mapper)
    {
        _UserRepository = UserRepository;
        _mapper = mapper;
    }
    public Task<QueryListResult<ObterTodosUsersQueryItem>>Handle(GetAllUserQueryInput request, CancellationToken cancellationToken)
    {
        var listUsers = _UserRepository.GetAsNoTracking().ToList().Select(c => new ObterTodosUsersQueryItem
        {
            Id = c.Id,
            Nome = c.Nome,
        });

        return Task.FromResult(new QueryListResult<ObterTodosUsersQueryItem>
        {
            Result = listUsers
        });
    }
}
