using Microsoft.EntityFrameworkCore;
using Pagamentos.Application.Interfaces;
using Pagamentos.Domain.Entities;
using Pagamentos.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pagamentos.Infrastructure.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly PagamentosDbContext _context;

        public TransacaoRepository(PagamentosDbContext context)
        {
            _context = context;
        }

        public async Task<Transacao> GetByIdAsync(Guid id)
        {
            return await _context.Transacoes.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transacao>> GetByPagamentoIdAsync(Guid pagamentoId)
        {
            return await _context.Transacoes
                .Where(t => t.PagamentoId == pagamentoId)
                .OrderByDescending(t => t.DataTransacao)
                .ToListAsync();
        }

        public async Task<Transacao> GetByReferenciaTidAsync(string referenciaTid)
        {
            return await _context.Transacoes
                .FirstOrDefaultAsync(t => t.ReferenciaTid == referenciaTid);
        }

        public async Task<IEnumerable<Transacao>> GetByStatusAsync(string status)
        {
            return await _context.Transacoes
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.DataTransacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transacao>> GetByTipoTransacaoAsync(string tipoTransacao)
        {
            return await _context.Transacoes
                .Where(t => t.TipoTransacao == tipoTransacao)
                .OrderByDescending(t => t.DataTransacao)
                .ToListAsync();
        }

        public async Task<Transacao> AddAsync(Transacao transacao)
        {
            await _context.Transacoes.AddAsync(transacao);
            await _context.SaveChangesAsync();
            return transacao;
        }

        public async Task UpdateAsync(Transacao transacao)
        {
            _context.Transacoes.Update(transacao);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var transacao = await GetByIdAsync(id);
            if (transacao != null)
            {
                _context.Transacoes.Remove(transacao);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Transacoes.AnyAsync(t => t.Id == id);
        }
    }
} 