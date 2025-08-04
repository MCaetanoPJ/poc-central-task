using CentralTask.Domain.Entidades.Base;
using CentralTask.Domain.Enums;
using CentralTask.Domain.Interfaces;
using CentralTask.Domain.Interfaces.Repositories.Base;
using CentralTask.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CentralTask.Infra.Data.Repositories.Base;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IEntidade
{
    private readonly CentralTaskContext _context;

    protected GenericRepository(CentralTaskContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public IQueryable<TEntity> Get() => _context.Set<TEntity>().AsTracking();

    public IQueryable<TEntity> GetAsNoTracking() => Get().AsNoTracking();

    public async Task<TEntity> GetByIdAsync(Guid id) => await Get().AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);

    public void Add(TEntity entity) => _context.Add(entity);

    public void Update(TEntity entity) => _context.Entry(entity).State = EntityState.Modified;

    public void Remove(TEntity entity) => _context.Remove(entity);

    public void UpdateRange(List<TEntity> entity) => _context.UpdateRange(entity);

    public void AddRange(List<TEntity> entity) => _context.AddRange(entity);

    public void RemoveRange(List<TEntity> entity) => _context.RemoveRange(entity);

    public bool Existe(Guid id) => GetAsNoTracking().Any(c => c.Id == id);
    public bool ExisteAtivo(Guid id) => GetAsNoTracking().Any(c => c.Id == id && c.Active == Status.Ativo);
}