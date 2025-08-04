using CentralTask.Domain.Entidades;
using CentralTask.Infra.Data.Mapping.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CentralTask.Infra.Data.Mapping
{
    public class TasksMapping : EntidadeMapping<Tasks>
    {
        public override void Configure(EntityTypeBuilder<Tasks> builder)
        {
            base.Configure(builder);
            builder.ToTable(ConstantesInfra.Tabelas.Tasks, ConstantesInfra.Schemas.Public);
            builder.HasKey(e => e.Id);

            builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
        }
    }
}