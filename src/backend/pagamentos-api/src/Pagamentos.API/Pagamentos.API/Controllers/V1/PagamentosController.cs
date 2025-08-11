using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagamentos.Application.Interfaces;
using Pagamentos.Application.ViewModels;
using Pagamentos.Core.Messages.CommonMessages.IntegrationEvents;
using Pagamentos.Core.Messages.CommonMessages.Notifications;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Pagamentos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/pagamentos")]
    [Authorize]
    public class PagamentosController : MainController
    {
        private readonly IMediator _mediator;
        private readonly IPagamentoConsultaAppService _pagamentoConsultaAppService;
        private readonly IPagamentoComandoAppService _pagamentoComandoAppService;

        public PagamentosController(IMediator mediator,
                                    IPagamentoConsultaAppService pagamentoConsultaAppService,
                                    IPagamentoComandoAppService pagamentoComandoAppService,
                                    NotificationContext _notificationContext) : base(mediator, _notificationContext)
        {
            _mediator = mediator;
            _pagamentoConsultaAppService = pagamentoConsultaAppService;
            _pagamentoComandoAppService = pagamentoComandoAppService;
        }

        [HttpPost("pagamento")]
        [SwaggerOperation(Summary = "Executa pagamento", Description = "Executa o pagamento do curso.")]
        [ProducesResponseType(typeof(PagamentoCursoInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Pagamento([FromBody] PagamentoCursoInputModel pagamento)
        {
            if (!ModelState.IsValid)
                return CustomResponse(HttpStatusCode.BadRequest);

            //var aluno = await _alunoConsultaAppService.ObterPorId(pagamento.AlunoId);
            //if (aluno == null)
            //    return NotFoundResponse("Aluno não encontrado.");


            //var matricula = await _alunoConsultaAppService.ObterMatriculaPorId(pagamento.MatriculaId);
            //if (matricula == null)
            //    return NotFoundResponse("Matrícula no curso não encontrada.");


            var pedidoEvent = new PagamentoCursoEvent(pagamento.MatriculaId,
                                                      pagamento.AlunoId,
                                                      pagamento.Total,
                                                      pagamento.NomeCartao,
                                                      pagamento.NumeroCartao,
                                                      pagamento.ExpiracaoCartao,
                                                      pagamento.CvvCartao);



            await _mediator.Publish(pedidoEvent);

            return CustomResponse(HttpStatusCode.OK);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter_todos")]
        [SwaggerOperation(Summary = "Obtém todos os pagamentos", Description = "Retorna uma lista com todos os pagamentos.")]
        [ProducesResponseType(typeof(IEnumerable<PagamentoViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult> ObterTodos()
        {
            var pagamentos = await _pagamentoConsultaAppService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, pagamentos);
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

            return CustomResponse(HttpStatusCode.OK, pagamento);
        }

        private IActionResult NotFoundResponse(string message)
        {
            NotificarErro(message);
            return CustomResponse(HttpStatusCode.NotFound);
        }
    }
}
