using Pagamentos.Core.DomainObjects;
using Pagamentos.Domain.Enum;

namespace Pagamentos.Domain.Entities
{
    public class Transacao : Entity
    {
        public Guid CobrancaCursoId { get; set; }
        public Guid PagamentoId { get; set; }
        public decimal Total { get; set; }
        public StatusTransacao StatusTransacao { get; set; }

        // EF. Rel.
        public Pagamento Pagamento { get; set; }
    }
}
