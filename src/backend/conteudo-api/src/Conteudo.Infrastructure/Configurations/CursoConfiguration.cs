using Conteudo.Domain.Entities;
using Core.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Conteudo.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> entity)
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

        entity.ToTable("Cursos");
        entity.HasKey(c => c.Id);

        entity.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(c => c.Valor)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        entity.Property(c => c.Nivel)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(c => c.Instrutor)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(c => c.ImagemUrl)
            .HasMaxLength(500);

        entity.Property(c => c.CreatedAt)
            .IsRequired();

        entity.Property(c => c.UpdatedAt)
            .IsRequired();

        // Configurar Value Object ConteudoProgramatico
        entity.OwnsOne(c => c.ConteudoProgramatico, cp =>
        {
            cp.Property(p => p.Resumo)
                .IsRequired()
                .HasColumnName("ConteudoProgramatico_Resumo");

            cp.Property(p => p.Descricao)
                .IsRequired()
                .HasColumnName("ConteudoProgramatico_Descricao");

            cp.Property(p => p.Objetivos)
                .IsRequired()
                .HasColumnName("ConteudoProgramatico_Objetivos");

            cp.Property(p => p.PreRequisitos)
                .HasColumnName("ConteudoProgramatico_PreRequisitos");

            cp.Property(p => p.PublicoAlvo)
                .HasColumnName("ConteudoProgramatico_PublicoAlvo");

            cp.Property(p => p.Metodologia)
                .HasColumnName("ConteudoProgramatico_Metodologia");

            cp.Property(p => p.Recursos)
                .HasColumnName("ConteudoProgramatico_Recursos");

            cp.Property(p => p.Avaliacao)
                .HasColumnName("ConteudoProgramatico_Avaliacao");

            cp.Property(p => p.Bibliografia)
                .HasColumnName("ConteudoProgramatico_Bibliografia");
        });

        // Relacionamento com Categoria
        entity.HasOne(c => c.Categoria)
            .WithMany(cat => cat.Cursos)
            .HasForeignKey(c => c.CategoriaId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relacionamento com Aulas
        entity.HasMany(c => c.Aulas)
            .WithOne(a => a.Curso)
            .HasForeignKey(a => a.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        entity.HasIndex(c => c.Nome).IsUnique();
        entity.HasIndex(c => c.Ativo);
        entity.HasIndex(c => c.ValidoAte);
        entity.HasIndex(c => c.CategoriaId);
    }
}
