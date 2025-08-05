using Microsoft.EntityFrameworkCore;
using Pagamentos.Application.Interfaces;
using Pagamentos.Domain.Entities;
using Pagamentos.Infrastructure.Data;

namespace Pagamentos.Infrastructure.Repositories
{
    public class ReembolsoRepository : IReembolsoRepository
    {
        private readonly PagamentosDbContext _context;

        public ReembolsoRepository(PagamentosDbContext context)
        {
            _context = context;
        }

        public async Task<Reembolso> GetByIdAsync(Guid id)
        {
            return await _context.Reembolsos.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reembolso>> GetByPagamentoIdAsync(Guid pagamentoId)
        {
            return await _context.Reembolsos
                .Where(r => r.PagamentoId == pagamentoId)
                .OrderByDescending(r => r.DataSolicitacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reembolso>> GetByStatusAsync(string status)
        {
            return await _context.Reembolsos
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.DataSolicitacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reembolso>> GetReembolsosPendentesAsync()
        {
            return await _context.Reembolsos
                .Where(r => r.Status == "Solicitado" || r.Status == "Aprovado")
                .OrderBy(r => r.DataSolicitacao)
                .ToListAsync();
        }

        public async Task<Reembolso> AddAsync(Reembolso reembolso)
        {
            await _context.Reembolsos.AddAsync(reembolso);
            await _context.SaveChangesAsync();
            return reembolso;
        }

        public async Task UpdateAsync(Reembolso reembolso)
        {
            _context.Reembolsos.Update(reembolso);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var reembolso = await GetByIdAsync(id);
            if (reembolso != null)
            {
                _context.Reembolsos.Remove(reembolso);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Reembolsos.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> ExistsByPagamentoIdAsync(Guid pagamentoId)
        {
            return await _context.Reembolsos.AnyAsync(r => r.PagamentoId == pagamentoId);
        }
    }
} 