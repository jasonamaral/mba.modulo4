using Microsoft.EntityFrameworkCore;
using Pagamentos.Application.Interfaces;
using Pagamentos.Domain.Entities;
using Pagamentos.Infrastructure.Data;

namespace Pagamentos.Infrastructure.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PagamentosDbContext _context;

        public PagamentoRepository(PagamentosDbContext context)
        {
            _context = context;
        }

        public async Task<Pagamento> GetByIdAsync(Guid id)
        {
            return await _context.Pagamentos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pagamento> GetByMatriculaIdAsync(Guid matriculaId)
        {
            return await _context.Pagamentos.FirstOrDefaultAsync(p => p.MatriculaId == matriculaId);
        }

        public async Task<Pagamento> GetByTransacaoIdAsync(string transacaoId)
        {
            return await _context.Pagamentos.FirstOrDefaultAsync(p => p.TransacaoId == transacaoId);
        }

        public async Task<IEnumerable<Pagamento>> GetByAlunoIdAsync(Guid alunoId)
        {
            return await _context.Pagamentos
                .Where(p => p.AlunoId == alunoId)
                .OrderByDescending(p => p.CriadoEm)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pagamento>> GetByStatusAsync(string status)
        {
            return await _context.Pagamentos
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.CriadoEm)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pagamento>> GetPagamentosPendentesAsync()
        {
            return await _context.Pagamentos
                .Where(p => p.Status == "Pendente" || p.Status == "Processando")
                .OrderBy(p => p.CriadoEm)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pagamento>> GetPagamentosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _context.Pagamentos
                .Where(p => p.CriadoEm >= dataInicio && p.CriadoEm <= dataFim)
                .OrderByDescending(p => p.CriadoEm)
                .ToListAsync();
        }

        public async Task<Pagamento> AddAsync(Pagamento pagamento)
        {
            await _context.Pagamentos.AddAsync(pagamento);
            await _context.SaveChangesAsync();
            return pagamento;
        }

        public async Task UpdateAsync(Pagamento pagamento)
        {
            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var pagamento = await GetByIdAsync(id);
            if (pagamento != null)
            {
                _context.Pagamentos.Remove(pagamento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountByAlunoAsync(Guid alunoId)
        {
            return await _context.Pagamentos.CountAsync(p => p.AlunoId == alunoId);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Pagamentos.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> ExistsByMatriculaIdAsync(Guid matriculaId)
        {
            return await _context.Pagamentos.AnyAsync(p => p.MatriculaId == matriculaId);
        }
    }
} 