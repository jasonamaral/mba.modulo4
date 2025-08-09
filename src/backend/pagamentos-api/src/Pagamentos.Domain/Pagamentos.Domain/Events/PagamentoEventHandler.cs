using MediatR;
using Pagamentos.Core.DomainObjects.DTO;
using Pagamentos.Core.Messages.CommonMessages.IntegrationEvents;
using Pagamentos.Domain.Interfaces;

namespace Pagamentos.Domain.Events
{
    public class PagamentoEventHandler : INotificationHandler<PagamentoCursoEvent>
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoEventHandler(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        public async Task Handle(PagamentoCursoEvent message, CancellationToken cancellationToken)
        {
            var pagamentoPedido = new PagamentoCurso
            {
                CursoId = message.PedidoId,
                ClienteId = message.AlunoId,
                Total = message.Total,
                NomeCartao = message.NomeCartao,
                NumeroCartao = message.NumeroCartao,
                ExpiracaoCartao = message.ExpiracaoCartao,
                CvvCartao = message.CvvCartao
            };

            await _pagamentoService.RealizarPagamentoPedido(pagamentoPedido);
        }
    }
}
