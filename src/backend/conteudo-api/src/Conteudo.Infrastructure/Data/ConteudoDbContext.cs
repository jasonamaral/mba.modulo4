using Conteudo.Domain.Common;
using Conteudo.Domain.Entities;
using Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Conteudo.Infrastructure.Data;

public class ConteudoDbContext : DbContext, IUnitOfWork
{
    public ConteudoDbContext(DbContextOptions<ConteudoDbContext> options) : base(options) { }

    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Aula> Aulas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Material> Materiais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCurso(modelBuilder);
        ConfigureAula(modelBuilder);
        ConfigureCategoria(modelBuilder);
        ConfigureMaterial(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureCurso(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Curso>(entity =>
        {
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
        });
    }

    private static void ConfigureAula(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aula>(entity =>
        {
            entity.ToTable("Aulas");
            entity.HasKey(a => a.Id);
            
            entity.Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(200);
                
            entity.Property(a => a.Descricao)
                .IsRequired();
                
            entity.Property(a => a.VideoUrl)
                .IsRequired()
                .HasMaxLength(500);
                
            entity.Property(a => a.TipoAula)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(a => a.Observacoes)
                .HasMaxLength(1000);
                
            entity.Property(a => a.CreatedAt)
                .IsRequired();
                
            entity.Property(a => a.UpdatedAt)
                .IsRequired();

            // Relacionamento com Curso
            entity.HasOne(a => a.Curso)
                .WithMany(c => c.Aulas)
                .HasForeignKey(a => a.CursoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Materiais
            entity.HasMany(a => a.Materiais)
                .WithOne(m => m.Aula)
                .HasForeignKey(m => m.AulaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(a => new { a.CursoId, a.Numero }).IsUnique();
            entity.HasIndex(a => a.IsPublicada);
            entity.HasIndex(a => a.TipoAula);
        });
    }

    private static void ConfigureCategoria(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categorias");
            entity.HasKey(c => c.Id);
            
            entity.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(c => c.Descricao)
                .IsRequired();
                
            entity.Property(c => c.Cor)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(c => c.IconeUrl)
                .HasMaxLength(500);
                
            entity.Property(c => c.CreatedAt)
                .IsRequired();
                
            entity.Property(c => c.UpdatedAt)
                .IsRequired();

            // Relacionamento com Cursos
            entity.HasMany(c => c.Cursos)
                .WithOne(curso => curso.Categoria)
                .HasForeignKey(curso => curso.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            entity.HasIndex(c => c.Nome).IsUnique();
            entity.HasIndex(c => c.IsAtiva);
            entity.HasIndex(c => c.Ordem);
        });
    }

    private static void ConfigureMaterial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>(entity =>
        {
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
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Entidade && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Entidade)entry.Entity;
            
            if (entry.State == EntityState.Modified)
            {
                entity.AtualizarDataModificacao();
            }
        }
    }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }
} 