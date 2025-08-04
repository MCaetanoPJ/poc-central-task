using CentralTask.Domain.Entidades;
using CentralTask.Domain.Entidades.Base;
using CentralTask.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace CentralTask.Infra.Data.Context;
public class CentralTaskContext :
    IdentityDbContext<User, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public CentralTaskContext(DbContextOptions<CentralTaskContext> options) : base(options)
    {

    }

    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var foreignKeys = entityType.GetForeignKeys()
                .Where(fk => fk.PrincipalEntityType.ClrType == typeof(User));

            foreach (var fk in foreignKeys)
            {
                fk.DeleteBehavior = DeleteBehavior.Cascade;
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        PreenchePropriedadesAuditaveis();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void PreenchePropriedadesAuditaveis()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    PreencheDataCriacao(entry);
                    break;
                case EntityState.Modified:
                    PreencheDataAtualizacao(entry);
                    break;
            }
        }
    }

    private static void PreencheDataCriacao(EntityEntry entry)
    {
        if (entry.Entity.GetType().GetProperty(nameof(Entidade.CreatedAt)) != null)
            entry.Property(nameof(Entidade.CreatedAt)).CurrentValue = DateTime.Now;
    }

    private static void PreencheDataAtualizacao(EntityEntry entry)
    {
        if (entry.Entity.GetType().GetProperty(nameof(Entidade.UpdatedAt)) != null)
            entry.Property(nameof(Entidade.UpdatedAt)).CurrentValue = DateTime.Now;
    }

    public async Task BeginTransaction(CancellationToken cancellationToken = default)
    {
        await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await Database.RollbackTransactionAsync(cancellationToken);
    }

    public void ChangeDateTimeBeforeSaveChange(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime)))
            {
                property.SetValueConverter(new DateTimeConverter());
            }
        }
    }
}
