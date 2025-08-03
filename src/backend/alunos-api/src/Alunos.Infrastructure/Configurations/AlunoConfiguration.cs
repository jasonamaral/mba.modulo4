using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Alunos.Domain.Entities;
using Core.Data.Constants;

namespace Alunos.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        #region Mapping columns
        builder.ToTable("Alunos");

        builder.HasKey(x => x.Id)
            .HasName("AlunosPK");

        builder.Property(x => x.Id)
            .HasColumnName("AlunoId")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.CodigoUsuarioAutenticacao)
            .HasColumnName("CodigoUsuarioAutenticacao")
            .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
            .IsRequired();

        builder.Property(x => x.Nome)
            .HasColumnName("Nome")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(100)
            .UseCollation(DatabaseTypeConstant.Collate)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("Email")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(100)
            .UseCollation(DatabaseTypeConstant.Collate)
            .IsRequired();

        builder.Property(x => x.Cpf)
            .HasColumnName("Cpf")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(11)
            .UseCollation(DatabaseTypeConstant.Collate);

        builder.Property(x => x.DataNascimento)
            .HasColumnName("DataNascimento")
            .HasColumnType(DatabaseTypeConstant.SmallDateTime)
            .IsRequired();

        builder.Property(x => x.Contato)
            .HasColumnName("Contato")
            .HasColumnType(DatabaseTypeConstant.Varchar)
            .HasMaxLength(25)
            .UseCollation(DatabaseTypeConstant.Collate);

        builder.Property(x => x.Ativo)
            .HasColumnName("Ativo")
            .HasColumnType(DatabaseTypeConstant.Boolean)
            .IsRequired();

        builder.Property(x => x.DataCriacao)
            .HasColumnName("DataCriacao")
            .HasColumnType(DatabaseTypeConstant.DateTime)
            .IsRequired();

        builder.Property(x => x.DataAlteracao)
            .HasColumnName("DataAlteracao")
            .HasColumnType(DatabaseTypeConstant.DateTime);
        #endregion

        #region Indexes
        builder.HasIndex(x => x.Nome).HasDatabaseName("AlunosNomeIDX");

        builder.HasIndex(x => x.Email)
               .IsUnique()
               .HasDatabaseName("AlunosEmailUK");
        #endregion

        #region Relationships
        builder.HasMany(x => x.MatriculasCursos)
           .WithOne(x => x.Aluno) 
           .HasForeignKey(x => x.AlunoId)
           .HasConstraintName("AlunosMatriculaCursoFK")
           .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}
