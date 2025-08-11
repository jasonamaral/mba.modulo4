using Core.Data;
using Microsoft.EntityFrameworkCore;
using Pagamentos.Domain.Entities;
using Pagamentos.Domain.Interfaces;
using Pagamentos.Infrastructure.Context;

namespace Pagamentos.Infrastructure.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PagamentoContext _context;

        public PagamentoRepository(PagamentoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;


        public void Adicionar(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
        }

        public void AdicionarTransacao(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<Pagamento> ObterPorId(Guid id)
        {
            return await _context.Pagamentos.Include(u => u.Transacao).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pagamento>> ObterTodos()
        {
            return await _context.Pagamentos.Include(u => u.Transacao).AsNoTracking().ToListAsync();
        }
    }
}
