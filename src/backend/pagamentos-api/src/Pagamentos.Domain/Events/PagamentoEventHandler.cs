using Core.Messages.Integration;
using MediatR;
using Pagamentos.Domain.Interfaces;
using Pagamentos.Domain.Models;

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
            var pagamentoCurso = new PagamentoCurso
            {
                CursoId = message.CursoId,
                ClienteId = message.AlunoId,
                Total = message.Total,
                NomeCartao = message.NomeCartao,
                NumeroCartao = message.NumeroCartao,
                ExpiracaoCartao = message.ExpiracaoCartao,
                CvvCartao = message.CvvCartao
            };

            await _pagamentoService.RealizarPagamento(pagamentoCurso);
        }
    }
}
