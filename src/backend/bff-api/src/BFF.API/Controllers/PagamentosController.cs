using BFF.API.Models.Request;
using BFF.API.Services.Conteudos;
using BFF.API.Services.Pagamentos;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs.Pagamentos.Response;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BFF.API.Controllers
{
    /// <summary>
    /// Controller de Pagamentos no BFF - Orquestra chamadas para Pagamento API
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PagamentosController : BffController
    {
        private readonly IPagamentoService _pagamentoService;
        private readonly IConteudoService _conteudoService;

        private readonly ILogger<PagamentosController> _logger;

        public PagamentosController(IMediatorHandler mediator,
                                    INotificationHandler<DomainNotificacaoRaiz> notifications,
                                    INotificador notificador,
                                    ILogger<PagamentosController> logger,
                                    IPagamentoService pagamentoService,
                                    IConteudoService conteudoService) : base(mediator, notifications, notificador)
        {
            _conteudoService = conteudoService;
            _pagamentoService = pagamentoService;
            _logger = logger;
        }

        [Authorize(Roles = "Usuario, Administrador")]
        [HttpPost("registrar-pagamento")]
        [ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Pagamento([FromBody] PagamentoCursoInputModel pagamento)
        {
            if (!ModelState.IsValid)
                return RespostaPadraoApi<CommandResult>(ModelState);

            try
            {
                var cursoResp = await _conteudoService.ObterCursoPorIdAsync(pagamento.CursoId, false);

                if (cursoResp?.Status == (int)HttpStatusCode.OK)
                {
                    var valorCurso = cursoResp.Data?.Valor ?? 0m;

                    if (valorCurso != pagamento.Total)
                        return RespostaPadraoApi(HttpStatusCode.BadRequest, "Valor do Pagamento diverge do Valor do Curso");
                }
                else
                {
                    return RespostaPadraoApi(HttpStatusCode.NotFound, "Curso não encontrado.");
                }

                var resultado = await _pagamentoService.ExecutarPagamento(pagamento);

                if (resultado?.Status == (int)HttpStatusCode.OK)
                {
                    return Ok(resultado);
                }

                return BadRequest(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar pagamento via BFF");
                return ProcessarErro(HttpStatusCode.InternalServerError, "Erro interno do servidor");
            }
        }

        [Authorize(Roles = "Usuario, Administrador")]
        [HttpGet("obter_todos")]
        [ProducesResponseType(typeof(ResponseResult<IEnumerable<PagamentoDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterTodos()
        {
            try
            {
                var pagamentos = await _pagamentoService.ObterTodos();
                return RespostaPadraoApi(HttpStatusCode.OK, pagamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter pagamentos via BFF");
                return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro ao obter pagamentos via BFF");
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("obter/{id:guid}")]
        [ProducesResponseType(typeof(ResponseResult<PagamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var pagamentos = await _pagamentoService.ObterPorIdPagamento(id);
                return RespostaPadraoApi(HttpStatusCode.OK, pagamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter pagamentos via BFF");
                return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro ao obter pagamentos via BFF");
            }
        }
    }
}
