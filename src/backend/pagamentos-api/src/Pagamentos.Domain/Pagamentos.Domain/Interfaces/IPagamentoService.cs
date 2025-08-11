using Pagamentos.Domain.Entities;
using Pagamentos.Domain.Models;

namespace Pagamentos.Domain.Interfaces
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamentoPedido(PagamentoCurso pagamentoPedido);
    }
}
