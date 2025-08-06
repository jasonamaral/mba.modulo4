using Pagamentos.Domain.Entities;

namespace Pagamentos.Application.Interfaces
{
    public interface IPagamentoRepository
    {
        Task<Pagamento> GetByIdAsync(Guid id);
        Task<Pagamento> GetByMatriculaIdAsync(Guid matriculaId);
        Task<Pagamento> GetByTransacaoIdAsync(string transacaoId);
        Task<IEnumerable<Pagamento>> GetByAlunoIdAsync(Guid alunoId);
        Task<IEnumerable<Pagamento>> GetByStatusAsync(string status);
        Task<IEnumerable<Pagamento>> GetPagamentosPendentesAsync();
        Task<IEnumerable<Pagamento>> GetPagamentosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<Pagamento> AddAsync(Pagamento pagamento);
        Task UpdateAsync(Pagamento pagamento);
        Task DeleteAsync(Guid id);
        Task<int> CountByAlunoAsync(Guid alunoId);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsByMatriculaIdAsync(Guid matriculaId);
    }
} 