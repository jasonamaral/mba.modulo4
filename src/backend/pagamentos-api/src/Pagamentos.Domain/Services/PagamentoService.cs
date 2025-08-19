using Core.Mediator;
using Core.Messages;
using Core.Messages.Integration;
using Pagamentos.Domain.Entities;
using Pagamentos.Domain.Enum;
using Pagamentos.Domain.Interfaces;
using Pagamentos.Domain.Models;

namespace Pagamentos.Domain.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoCartaoCreditoFacade _pagamentoCartaoCreditoFacade;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PagamentoService(IPagamentoCartaoCreditoFacade pagamentoCartaoCreditoFacade,
                                IPagamentoRepository pagamentoRepository,
                                IMediatorHandler mediatorHandler)
        {
            _pagamentoCartaoCreditoFacade = pagamentoCartaoCreditoFacade;
            _pagamentoRepository = pagamentoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<Transacao> RealizarPagamento(PagamentoCurso pagamentoAnuidade)
        {
            var cobranca = new CobrancaCurso
            {
                Id = pagamentoAnuidade.CursoId,
                Valor = pagamentoAnuidade.Total
            };

            var pagamento = new Pagamento
            {
                Valor = pagamentoAnuidade.Total,
                NomeCartao = pagamentoAnuidade.NomeCartao,
                NumeroCartao = pagamentoAnuidade.NumeroCartao,
                ExpiracaoCartao = pagamentoAnuidade.ExpiracaoCartao,
                CvvCartao = pagamentoAnuidade.CvvCartao,
                CobrancaCursoId = pagamentoAnuidade.CursoId,
                AlunoId = pagamentoAnuidade.ClienteId
            };

            var transacao = _pagamentoCartaoCreditoFacade.RealizarPagamento(cobranca, pagamento);

            if (transacao.StatusTransacao == StatusTransacao.Pago)
            {
                //TODO
                await _mediatorHandler.PublicarEvento(new PagamentoRealizadoEvent(cobranca.Id, pagamentoAnuidade.ClienteId, transacao.PagamentoId, transacao.Id, cobranca.Valor));

                pagamento.Status = transacao.StatusTransacao.ToString();
                pagamento.Transacao = transacao;

                _pagamentoRepository.Adicionar(pagamento);
                _pagamentoRepository.AdicionarTransacao(transacao);

                await _pagamentoRepository.UnitOfWork.Commit();
                return transacao;
            }

            await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz("pagamento", "A operadora recusou o pagamento"));
            await _mediatorHandler.PublicarEvento(new PagamentoRecusadoEvent(cobranca.Id, pagamentoAnuidade.ClienteId, transacao.PagamentoId, transacao.Id, cobranca.Valor));

            return transacao;
        }
    }
}
