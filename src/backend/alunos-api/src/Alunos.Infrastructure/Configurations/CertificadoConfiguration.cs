using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Alunos.Domain.Entities;
using Core.Data.Constants;

namespace Alunos.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class CertificadoConfiguration : IEntityTypeConfiguration<Certificado>
{
    public void Configure(EntityTypeBuilder<Certificado> builder)
    {
        #region Mapping columns
        builder.ToTable("Certificados");

        builder.HasKey(x => x.Id)
            .HasName("CertificadosPK");

        builder.Property(x => x.Id)
            .HasColumnName("CertificadoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.MatriculaCursoId)
            .HasColumnName("MatriculaCursoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.NomeCurso)
            .HasColumnName("NomeCurso")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.DataSolicitacao)
            .HasColumnName("DataSolicitacao")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime)
            .IsRequired();

        builder.Property(x => x.DataEmissao)
            .HasColumnName("DataEmissao")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime);

        builder.Property(x => x.CargaHoraria)
            .HasColumnName("CargaHoraria")
            .HasColumnType(DatabaseTypeConstant.Int16)
            .IsRequired();

        builder.Property(x => x.NotaFinal)
            .HasColumnName("NotaFinal")
            .HasColumnType(DatabaseTypeConstant.Byte)
            .IsRequired();

        builder.Property(x => x.PathCertificado)
            .HasColumnName("PathCertificado")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.NomeInstrutor)
            .HasColumnName("NomeInstrutor")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("DataCriacao")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("DataAlteracao")
            .HasColumnType(DatabaseTypeConstant.DateTime);
        #endregion Mapping columns

        #region Indexes
        builder.HasIndex(x => x.MatriculaCursoId).HasDatabaseName("CertificadosMatriculaCursoIdIDX");
        #endregion Indexes

        #region Relationships
        builder.HasOne(x => x.MatriculaCurso)
           .WithOne(x => x.Certificado)
           .HasForeignKey<Certificado>(x => x.MatriculaCursoId)
           .HasConstraintName("CertificadoMatriculaCursoFK")
           .OnDelete(DeleteBehavior.Cascade);
        #endregion Relationships
    }
}
