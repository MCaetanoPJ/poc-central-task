using CentralTask.Domain.Entidades;
using CentralTask.Infra.Data.Mapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CentralTask.Infra.Data.Mapping;

public class UserMapping : EntidadeMapping<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable(ConstantesInfra.Tabelas.User, ConstantesInfra.Schemas.Public);

        builder.Property(x => x.Nome)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(250)
            .IsRequired();
    }
}