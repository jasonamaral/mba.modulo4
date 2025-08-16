using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Messages.Integration;
using Core.Notification;
using Core.Services.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagamentos.Application.Interfaces;
using Pagamentos.Application.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Pagamentos.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/pagamentos")]
    [Authorize]

    public class PagamentosController(IPagamentoConsultaAppService pagamentoConsultaAppService,
                                      IPagamentoComandoAppService pagamentoComandoAppService,
                                      IMediatorHandler mediator,
                                      INotificador notificador,
                                      INotificationHandler<DomainNotificacaoRaiz> notifications) : MainController(mediator, notifications, notificador)
    {
        private readonly IMediatorHandler _mediator = mediator;
        private readonly IPagamentoConsultaAppService _pagamentoConsultaAppService = pagamentoConsultaAppService;
        private readonly IPagamentoComandoAppService _pagamentoComandoAppService = pagamentoComandoAppService;



        [HttpPost("pagamento")]
        [SwaggerOperation(Summary = "Executa pagamento", Description = "Executa o pagamento do curso.")]
        [ProducesResponseType(typeof(PagamentoCursoInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Pagamento([FromBody] PagamentoCursoInputModel pagamento)
        {
            if (!ModelState.IsValid)
            {
                return RespostaPadraoApi<CommandResult>(ModelState);
            }


            var evento = new PagamentoCursoEvent(pagamento.MatriculaId,
                                                    pagamento.AlunoId,
                                                    pagamento.Total,
                                                    pagamento.NomeCartao,
                                                    pagamento.NumeroCartao,
                                                    pagamento.ExpiracaoCartao,
                                                    pagamento.CvvCartao);



            await _mediator.PublicarEvento(evento);

            return RespostaPadraoApi(HttpStatusCode.OK, "");

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter_todos")]
        [SwaggerOperation(Summary = "Obtém todos os pagamentos", Description = "Retorna uma lista com todos os pagamentos.")]
        [ProducesResponseType(typeof(IEnumerable<PagamentoViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult> ObterTodos()
        {
            var pagamentos = await _pagamentoConsultaAppService.ObterTodos();
            return RespostaPadraoApi(HttpStatusCode.OK, pagamentos);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter/{id:guid}")]
        [SwaggerOperation(Summary = "Obtém pagamento por ID", Description = "Retorna os dados de um pagamento específico.")]
        [ProducesResponseType(typeof(PagamentoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var pagamento = await _pagamentoConsultaAppService.ObterPorId(id);
            if (pagamento == null)
                return NotFoundResponse("Pagamento não encontrado.");

            return RespostaPadraoApi(HttpStatusCode.OK, pagamento);
        }

        private IActionResult NotFoundResponse(string message)
        {
            _notificador.AdicionarErro(message);
            return RespostaPadraoApi(HttpStatusCode.NotFound, message);
        }
    }

}