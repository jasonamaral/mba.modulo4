using Microsoft.EntityFrameworkCore;
using Pagamentos.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Pagamentos.Infrastructure.Data
{
    public class PagamentosDbContext : DbContext
    {
        public PagamentosDbContext(DbContextOptions<PagamentosDbContext> options) : base(options)
        {
        }

        // Tabelas do domínio
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Reembolso> Reembolsos { get; set; }
        public DbSet<Webhook> Webhooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Pagamento
            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.ToTable("Pagamentos");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                
                entity.Property(p => p.MatriculaId).IsRequired();
                entity.Property(p => p.AlunoId).IsRequired();
                entity.Property(p => p.CursoId).IsRequired();
                entity.Property(p => p.Valor).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(p => p.Status).HasMaxLength(50).IsRequired();
                entity.Property(p => p.MetodoPagamento).HasMaxLength(50).IsRequired();
                entity.Property(p => p.TransacaoId).HasMaxLength(100);
                entity.Property(p => p.GatewayPagamento).HasMaxLength(100).IsRequired();
                entity.Property(p => p.DataPagamento);
                entity.Property(p => p.DataVencimento);
                entity.Property(p => p.CriadoEm).IsRequired();
                entity.Property(p => p.AtualizadoEm).IsRequired();
                
                // Dados do cartão
                entity.Property(p => p.NumeroCartao).HasMaxLength(20);
                entity.Property(p => p.NomeTitularCartao).HasMaxLength(100);
                entity.Property(p => p.ValidadeCartao).HasMaxLength(7);
                entity.Property(p => p.CvvCartao).HasMaxLength(4);
                
                // Dados PIX
                entity.Property(p => p.ChavePix).HasMaxLength(100);
                entity.Property(p => p.QrCodePix).HasMaxLength(500);
                
                // Dados Boleto
                entity.Property(p => p.LinhaDigitavel).HasMaxLength(100);
                entity.Property(p => p.CodigoBarras).HasMaxLength(100);
                
                // Índices
                entity.HasIndex(p => p.MatriculaId).IsUnique();
                entity.HasIndex(p => p.AlunoId);
                entity.HasIndex(p => p.CursoId);
                entity.HasIndex(p => p.TransacaoId);
                entity.HasIndex(p => p.Status);
                entity.HasIndex(p => p.MetodoPagamento);
                entity.HasIndex(p => p.GatewayPagamento);
                entity.HasIndex(p => p.DataPagamento);
                entity.HasIndex(p => p.CriadoEm);
            });

            // Configuração da entidade Transacao
            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.ToTable("Transacoes");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).ValueGeneratedOnAdd();
                
                entity.Property(t => t.PagamentoId).IsRequired();
                entity.Property(t => t.TipoTransacao).HasMaxLength(100).IsRequired();
                entity.Property(t => t.Valor).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(t => t.Status).HasMaxLength(50).IsRequired();
                entity.Property(t => t.ReferenciaTid).HasMaxLength(100);
                entity.Property(t => t.AutorizacaoId).HasMaxLength(100);
                entity.Property(t => t.ResponseJson);
                entity.Property(t => t.DataTransacao).IsRequired();
                entity.Property(t => t.CriadoEm).IsRequired();
                entity.Property(t => t.AtualizadoEm).IsRequired();
                
                // Relacionamento com Pagamento
                entity.HasOne<Pagamento>()
                      .WithMany()
                      .HasForeignKey(t => t.PagamentoId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Índices
                entity.HasIndex(t => t.PagamentoId);
                entity.HasIndex(t => t.TipoTransacao);
                entity.HasIndex(t => t.Status);
                entity.HasIndex(t => t.ReferenciaTid);
                entity.HasIndex(t => t.DataTransacao);
            });

            // Configuração da entidade Reembolso
            modelBuilder.Entity<Reembolso>(entity =>
            {
                entity.ToTable("Reembolsos");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();
                
                entity.Property(r => r.PagamentoId).IsRequired();
                entity.Property(r => r.Valor).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(r => r.Status).HasMaxLength(50).IsRequired();
                entity.Property(r => r.Motivo).HasMaxLength(500);
                entity.Property(r => r.ReembolsoId).HasMaxLength(100);
                entity.Property(r => r.DataSolicitacao).IsRequired();
                entity.Property(r => r.DataProcessamento);
                entity.Property(r => r.MotivoRejeicao).HasMaxLength(500);
                entity.Property(r => r.CriadoEm).IsRequired();
                entity.Property(r => r.AtualizadoEm).IsRequired();
                
                // Relacionamento com Pagamento
                entity.HasOne<Pagamento>()
                      .WithMany()
                      .HasForeignKey(r => r.PagamentoId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Índices
                entity.HasIndex(r => r.PagamentoId);
                entity.HasIndex(r => r.Status);
                entity.HasIndex(r => r.DataSolicitacao);
                entity.HasIndex(r => r.DataProcessamento);
            });

            // Configuração da entidade Webhook
            modelBuilder.Entity<Webhook>(entity =>
            {
                entity.ToTable("Webhooks");
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Id).ValueGeneratedOnAdd();
                
                entity.Property(w => w.PagamentoId);
                entity.Property(w => w.Origem).HasMaxLength(100).IsRequired();
                entity.Property(w => w.Evento).HasMaxLength(50).IsRequired();
                entity.Property(w => w.Payload).IsRequired();
                entity.Property(w => w.Status).HasMaxLength(50).IsRequired();
                entity.Property(w => w.DataRecebimento).IsRequired();
                entity.Property(w => w.DataProcessamento);
                entity.Property(w => w.TentativasProcessamento).HasDefaultValue(0);
                entity.Property(w => w.ErroProcessamento).HasMaxLength(1000);
                entity.Property(w => w.CriadoEm).IsRequired();
                entity.Property(w => w.AtualizadoEm).IsRequired();
                
                // Relacionamento com Pagamento
                entity.HasOne<Pagamento>()
                      .WithMany()
                      .HasForeignKey(w => w.PagamentoId)
                      .OnDelete(DeleteBehavior.SetNull);
                
                // Índices
                entity.HasIndex(w => w.PagamentoId);
                entity.HasIndex(w => w.Origem);
                entity.HasIndex(w => w.Evento);
                entity.HasIndex(w => w.Status);
                entity.HasIndex(w => w.DataRecebimento);
                entity.HasIndex(w => w.DataProcessamento);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Domain.Common.Entidade>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.DefinirCriadoEm(DateTime.UtcNow);
                        entry.Entity.DefinirAtualizadoEm(DateTime.UtcNow);
                        break;
                    case EntityState.Modified:
                        entry.Entity.AtualizarDataModificacao();
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
} 