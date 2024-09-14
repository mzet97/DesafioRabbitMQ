using Desafio.ProtocoloAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desafio.ProtocoloAPI.Infrastructure.Persistence.Mappings;

public class ProtocoloMapping : IEntityTypeConfiguration<Protocolo>
{
    public void Configure(EntityTypeBuilder<Protocolo> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(c => c.NumeroProtocolo)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(c => c.NumeroVia)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(c => c.Cpf)
            .IsRequired()
            .HasColumnType("varchar(11)");

        builder.Property(c => c.Rg)
            .IsRequired()
            .HasColumnType("varchar(9)");

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(c => c.NomeMae)
           .HasColumnType("varchar(100)");

        builder.Property(c => c.NomePai)
           .HasColumnType("varchar(100)");

        builder.Property(c => c.Foto)
           .IsRequired()
           .HasColumnType("bytea");

        builder.Property(p => p.CreatedAt)
           .IsRequired(true);

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false);

        builder.Property(p => p.DeletedAt)
            .IsRequired(false);

        builder.HasIndex(p => new { p.Cpf, p.NumeroVia })
           .IsUnique();

        builder.HasIndex(p => new { p.Rg, p.NumeroVia })
            .IsUnique();
    }
}
