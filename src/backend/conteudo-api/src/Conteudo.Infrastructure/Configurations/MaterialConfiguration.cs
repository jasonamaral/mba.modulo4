using Conteudo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conteudo.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> entity)
    {
        #region Mapping columns
        //builder.ToTable("Alunos");

        //builder.HasKey(x => x.Id)
        //    .HasName("AlunosPK");

        //builder.Property(x => x.Id)
        //    .HasColumnName("AlunoId")
        //    .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
        //    .IsRequired();

        //builder.Property(x => x.CodigoUsuarioAutenticacao)
        //    .HasColumnName("CodigoUsuarioAutenticacao")
        //    .HasColumnType(DatabaseTypeConstant.UniqueIdentifier)
        //    .IsRequired();

        //builder.Property(x => x.Nome)
        //    .HasColumnName("Nome")
        //    .HasColumnType(DatabaseTypeConstant.Varchar)
        //    .HasMaxLength(100)
        //    .IsRequired();

        //builder.Property(x => x.Email)
        //    .HasColumnName("Email")
        //    .HasColumnType(DatabaseTypeConstant.Varchar)
        //    .HasMaxLength(100)
        //    .IsRequired();

        //builder.Property(x => x.Cpf)
        //    .HasColumnName("Cpf")
        //    .HasColumnType(DatabaseTypeConstant.Varchar)
        //    .HasMaxLength(11);

        //builder.Property(x => x.DataNascimento)
        //    .HasColumnName("DataNascimento")
        //    .HasColumnType(DatabaseTypeConstant.SmallDateTime)
        //    .IsRequired();

        //builder.Property(x => x.Telefone)
        //    .HasColumnName("Telefone")
        //    .HasColumnType(DatabaseTypeConstant.Varchar)
        //    .HasMaxLength(25);

        //builder.Property(x => x.Ativo)
        //    .HasColumnName("Ativo")
        //    .HasColumnType(DatabaseTypeConstant.Boolean)
        //    .IsRequired();

        //builder.Property(x => x.Genero)
        //    .HasColumnName("Genero")
        //    .HasColumnType(DatabaseTypeConstant.Varchar)
        //    .HasMaxLength(20)
        //    .IsRequired();

        //builder.Property(x => x.Cidade)
        //    .HasColumnName("Cidade")
        //    .HasColumnType(DatabaseTypeConstant.Varchar)
        //    .HasMaxLength(50)
        //    .IsRequired();

        //builder.Property(x => x.Estado)
        //    .HasColumnName("Estado")
        //    .HasColumnType(DatabaseTypeConstant.Varchar)
        //    .HasMaxLength(2);

        //builder.Property(x => x.Cep)
        //    .HasColumnName("Cep")
        //    .HasColumnType(DatabaseTypeConstant.Varchar)
        //    .HasMaxLength(8)
        //    .IsRequired();

        //builder.Property(x => x.Foto)
        //    .HasColumnName("Foto")
        //    .HasColumnType(DatabaseTypeConstant.Varchar)
        //    .HasMaxLength(1024);

        //builder.Property(x => x.CreatedAt)
        //    .HasColumnName("DataCriacao")
        //    .HasColumnType(DatabaseTypeConstant.DateTime)
        //    .IsRequired();

        //builder.Property(x => x.UpdatedAt)
        //    .HasColumnName("DataAlteracao")
        //    .HasColumnType(DatabaseTypeConstant.DateTime);
        //#endregion

        //#region Indexes
        //builder.HasIndex(x => x.Nome).HasDatabaseName("AlunosNomeIDX");

        //builder.HasIndex(x => x.Email)
        //       .IsUnique()
        //       .HasDatabaseName("AlunosEmailUK");
        //#endregion

        //#region Relationships
        //builder.HasMany(x => x.MatriculasCursos)
        //   .WithOne(x => x.Aluno)
        //   .HasForeignKey(x => x.AlunoId)
        //   .HasConstraintName("AlunosMatriculaCursoFK")
        //   .OnDelete(DeleteBehavior.Cascade);
        #endregion

        entity.ToTable("Materiais");
        entity.HasKey(m => m.Id);

        entity.Property(m => m.Nome)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(m => m.Descricao)
            .IsRequired();

        entity.Property(m => m.TipoMaterial)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(m => m.Url)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(m => m.Extensao)
            .HasMaxLength(10);

        entity.Property(m => m.CreatedAt)
            .IsRequired();

        entity.Property(m => m.UpdatedAt)
            .IsRequired();

        // Relacionamento com Aula
        entity.HasOne(m => m.Aula)
            .WithMany(a => a.Materiais)
            .HasForeignKey(m => m.AulaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        entity.HasIndex(m => new { m.AulaId, m.Nome }).IsUnique();
        entity.HasIndex(m => m.IsAtivo);
        entity.HasIndex(m => m.TipoMaterial);
        entity.HasIndex(m => m.Ordem);
    }
}
