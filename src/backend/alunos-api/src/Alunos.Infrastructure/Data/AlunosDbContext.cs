using Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Alunos.Infrastructure.Data;

public class AlunosDbContext : DbContext
{
    public AlunosDbContext(DbContextOptions<AlunosDbContext> options) : base(options)
    {
    }

    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<MatriculaCurso> Matriculas { get; set; }
    public DbSet<Certificado> Certificados { get; set; }
    public DbSet<HistoricoAluno> HistoricoAlunos { get; set; }
    public DbSet<Progresso> Progressos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Aluno
        modelBuilder.Entity<Aluno>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CodigoUsuarioAutenticacao).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.CodigoUsuarioAutenticacao).IsUnique();
        });

        // Configuração da entidade MatriculaCurso
        modelBuilder.Entity<MatriculaCurso>(entity =>
        {
            entity.HasKey(e => e.Id);
            // Configuração de relacionamento será feita posteriormente
        });

        // Configuração da entidade Certificado
        modelBuilder.Entity<Certificado>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Matricula)
                  .WithOne()
                  .HasForeignKey<Certificado>("MatriculaId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade HistoricoAluno
        modelBuilder.Entity<HistoricoAluno>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Aluno)
                  .WithMany()
                  .HasForeignKey("AlunoId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade Progresso
        modelBuilder.Entity<Progresso>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Matricula)
                  .WithMany()
                  .HasForeignKey("MatriculaId")
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}