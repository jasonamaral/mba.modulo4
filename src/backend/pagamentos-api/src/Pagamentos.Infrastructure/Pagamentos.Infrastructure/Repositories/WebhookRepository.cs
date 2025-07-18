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
    public class WebhookRepository : IWebhookRepository
    {
        private readonly PagamentosDbContext _context;

        public WebhookRepository(PagamentosDbContext context)
        {
            _context = context;
        }

        public async Task<Webhook> GetByIdAsync(Guid id)
        {
            return await _context.Webhooks.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Webhook>> GetByPagamentoIdAsync(Guid pagamentoId)
        {
            return await _context.Webhooks
                .Where(w => w.PagamentoId == pagamentoId)
                .OrderByDescending(w => w.DataRecebimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Webhook>> GetByOrigemAsync(string origem)
        {
            return await _context.Webhooks
                .Where(w => w.Origem == origem)
                .OrderByDescending(w => w.DataRecebimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Webhook>> GetByStatusAsync(string status)
        {
            return await _context.Webhooks
                .Where(w => w.Status == status)
                .OrderByDescending(w => w.DataRecebimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Webhook>> GetWebhooksPendentesAsync()
        {
            return await _context.Webhooks
                .Where(w => w.Status == "Recebido" || w.Status == "Processando")
                .OrderBy(w => w.DataRecebimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Webhook>> GetWebhooksParaReprocessarAsync()
        {
            return await _context.Webhooks
                .Where(w => w.Status == "Falha" && w.TentativasProcessamento < 5)
                .OrderBy(w => w.DataRecebimento)
                .ToListAsync();
        }

        public async Task<Webhook> AddAsync(Webhook webhook)
        {
            await _context.Webhooks.AddAsync(webhook);
            await _context.SaveChangesAsync();
            return webhook;
        }

        public async Task UpdateAsync(Webhook webhook)
        {
            _context.Webhooks.Update(webhook);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var webhook = await GetByIdAsync(id);
            if (webhook != null)
            {
                _context.Webhooks.Remove(webhook);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Webhooks.AnyAsync(w => w.Id == id);
        }
    }
} 