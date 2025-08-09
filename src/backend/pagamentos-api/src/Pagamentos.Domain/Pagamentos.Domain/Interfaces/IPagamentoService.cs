using Pagamentos.Core.DomainObjects.DTO;
using Pagamentos.Domain.Entities;

namespace Pagamentos.Domain.Interfaces
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamentoPedido(PagamentoCurso pagamentoPedido);
    }
}
