using Domain.Entities;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings
{
    public class ParticipantMap : BaseEntityMap<Participant>
    {
        public override void Map(EntityTypeBuilder<Participant> builder)
        {
            builder.ToTable("Participant");

            builder.Property(c => c.Name).HasColumnName("Name").HasMaxLength(100);
            builder.Property(c => c.Cpf).HasColumnName("Cpf").HasMaxLength(11);
        }
    }
}