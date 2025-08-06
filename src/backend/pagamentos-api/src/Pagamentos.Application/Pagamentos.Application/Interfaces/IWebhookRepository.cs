using Pagamentos.Domain.Entities;

namespace Pagamentos.Application.Interfaces
{
    public interface IWebhookRepository
    {
        Task<Webhook> GetByIdAsync(Guid id);
        Task<IEnumerable<Webhook>> GetByPagamentoIdAsync(Guid pagamentoId);
        Task<IEnumerable<Webhook>> GetByOrigemAsync(string origem);
        Task<IEnumerable<Webhook>> GetByStatusAsync(string status);
        Task<IEnumerable<Webhook>> GetWebhooksPendentesAsync();
        Task<IEnumerable<Webhook>> GetWebhooksParaReprocessarAsync();
        Task<Webhook> AddAsync(Webhook webhook);
        Task UpdateAsync(Webhook webhook);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
} 