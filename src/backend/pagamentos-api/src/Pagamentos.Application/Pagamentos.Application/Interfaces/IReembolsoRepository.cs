using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pagamentos.Domain.Entities;

namespace Pagamentos.Application.Interfaces
{
    public interface IReembolsoRepository
    {
        Task<Reembolso> GetByIdAsync(Guid id);
        Task<IEnumerable<Reembolso>> GetByPagamentoIdAsync(Guid pagamentoId);
        Task<IEnumerable<Reembolso>> GetByStatusAsync(string status);
        Task<IEnumerable<Reembolso>> GetReembolsosPendentesAsync();
        Task<Reembolso> AddAsync(Reembolso reembolso);
        Task UpdateAsync(Reembolso reembolso);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsByPagamentoIdAsync(Guid pagamentoId);
    }
} 