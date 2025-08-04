using CentralTask.Domain.Entidades.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CentralTask.Infra.Data.Mapping.Base;

public abstract class EntidadeMapping<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IEntidade
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
