using CentralTask.Domain.Entidades.Base;

namespace CentralTask.Domain.Interfaces.Repositories.Base;

public interface IGenericRepository<TEntity> where TEntity : class, IEntidade
{
    IUnitOfWork UnitOfWork { get; }
    void Add(TEntity entity);
    void AddRange(List<TEntity> entity);
    IQueryable<TEntity> Get();
    IQueryable<TEntity> GetAsNoTracking();
    Task<TEntity> GetByIdAsync(Guid id);
    void Update(TEntity entity);
    void UpdateRange(List<TEntity> entity);
    void Remove(TEntity entity);
    void RemoveRange(List<TEntity> entity);
    bool Existe(Guid id);
    bool ExisteAtivo(Guid id);

}