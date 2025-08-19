using Conteudo.Domain.Entities;
using Core.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Conteudo.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> entity)
    {
        #region Mapping columns

        entity.ToTable("Materiais");

        entity.HasKey(a => a.Id)
            .HasName("MateriaisPK");

        entity.Property(x => x.Id)
            .HasColumnName("MaterialId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        entity.Property(m => m.Nome)
            .HasColumnName("Nome")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(m => m.Descricao)
            .HasColumnName("Descricao")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .IsRequired();

        entity.Property(m => m.TipoMaterial)
            .HasColumnName("TipoMaterial")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(m => m.Url)
            .HasColumnName("Url")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(m => m.Extensao)
            .HasColumnName("Extensao")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(10);

        entity.Property(a => a.CreatedAt)
            .HasColumnName("DataCriacao")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();

        entity.Property(a => a.UpdatedAt)
            .HasColumnName("DataAlteracao")
            .HasColumnType(DatabaseTypeConstant.DateTime);

        #endregion Mapping columns

        #region Indexes

        // Índices
        entity.HasIndex(m => new { m.AulaId, m.Nome })
            .HasDatabaseName("MaterialAulaIdNomeIDX")
            .IsUnique();

        entity.HasIndex(m => m.IsAtivo)
            .HasDatabaseName("MaterialIsAtivoIDX");

        entity.HasIndex(m => m.TipoMaterial)
            .HasDatabaseName("MaterialTipoMaterialIDX");

        entity.HasIndex(m => m.Ordem)
            .HasDatabaseName("MaterialOrdemIDX");

        #endregion Indexes

        #region Relationships

        // Relacionamento com Aula
        entity.HasOne(m => m.Aula)
            .WithMany(a => a.Materiais)
            .HasForeignKey(m => m.AulaId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion Relationships
    }
}
