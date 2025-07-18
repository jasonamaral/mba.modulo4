using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pagamentos.Domain.Entities;

namespace Pagamentos.Application.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<Transacao> GetByIdAsync(Guid id);
        Task<IEnumerable<Transacao>> GetByPagamentoIdAsync(Guid pagamentoId);
        Task<Transacao> GetByReferenciaTidAsync(string referenciaTid);
        Task<IEnumerable<Transacao>> GetByStatusAsync(string status);
        Task<IEnumerable<Transacao>> GetByTipoTransacaoAsync(string tipoTransacao);
        Task<Transacao> AddAsync(Transacao transacao);
        Task UpdateAsync(Transacao transacao);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
} 