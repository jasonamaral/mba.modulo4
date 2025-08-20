using Conteudo.Application.Commands.AtualizarAula;
using Conteudo.Application.Commands.CadastrarAula;
using Conteudo.Application.Commands.DespublicarAula;
using Conteudo.Application.Commands.ExcluirAula;
using Conteudo.Application.Commands.PublicarAula;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using Core.Services.Controllers;
using Core.SharedDtos.Conteudo;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Conteudo.API.Controllers
{
    /// <summary>
    /// Controller para gerenciar aulas
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class AulaController(IMediatorHandler mediator,
                                IAulaAppService aulaAppService,
                                INotificationHandler<DomainNotificacaoRaiz> notifications,
                                INotificador notificador) : MainController(mediator, notifications, notificador)
    {
        private readonly IMediatorHandler _mediator = mediator;
        private readonly IAulaAppService _aulaAppService = aulaAppService;

        /// <summary>
        /// Obtém uma aula por ID
        /// </summary>
        /// <param name="id">ID da aula</param>
        /// <param name="includeMateriais">Se deve incluir materiais na resposta</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseResult<AulaDto>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 404)]
        [Authorize(Roles = "Usuario, Administrador")]
        public async Task<IActionResult> ObterPorId(Guid id, [FromQuery] bool includeMateriais = false)
        {
            try
            {
                var aula = await _aulaAppService.ObterPorIdAsync(id, includeMateriais);

                if (aula == null)
                {
                    _notificador.AdicionarErro("Aula não encontrada.");
                    return RespostaPadraoApi<string>();
                }

                return RespostaPadraoApi(data: aula);
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Obtém todas as aulas
        /// </summary>
        /// <param name="includeMateriais">Se deve incluir materiais na resposta</param>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        [Authorize(Roles = "Usuario, Administrador")]
        public async Task<IActionResult> ObterTodos([FromQuery] bool includeMateriais = false)
        {
            try
            {
                var aulas = await _aulaAppService.ObterTodosAsync(includeMateriais);
                return RespostaPadraoApi(data: aulas);
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Obtém aulas por curso
        /// </summary>
        /// <param name="cursoId">ID do curso</param>
        /// <param name="includeMateriais">Se deve incluir materiais na resposta</param>
        [HttpGet("curso/{cursoId}")]
        [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        [Authorize(Roles = "Usuario, Administrador")]
        public async Task<IActionResult> ObterPorCursoId(Guid cursoId, [FromQuery] bool includeMateriais = false)
        {
            try
            {
                if (cursoId == Guid.Empty)
                    return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do curso inválido");

                var aulas = await _aulaAppService.ObterPorCursoIdAsync(cursoId, includeMateriais);
                return RespostaPadraoApi(data: aulas);
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Obtém aulas publicadas
        /// </summary>
        /// <param name="includeMateriais">Se deve incluir materiais na resposta</param>
        [HttpGet("publicadas")]
        [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        [Authorize(Roles = "Usuario, Administrador")]
        public async Task<IActionResult> ObterPublicadas([FromQuery] bool includeMateriais = false)
        {
            try
            {
                var aulas = await _aulaAppService.ObterPublicadasAsync(includeMateriais);
                return RespostaPadraoApi(data: aulas);
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Obtém aulas publicadas por curso
        /// </summary>
        /// <param name="cursoId">ID do curso</param>
        /// <param name="includeMateriais">Se deve incluir materiais na resposta</param>
        [HttpGet("curso/{cursoId}/publicadas")]
        [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        [Authorize(Roles = "Usuario, Administrador")]
        public async Task<IActionResult> ObterPublicadasPorCursoId(Guid cursoId, [FromQuery] bool includeMateriais = false)
        {
            try
            {
                if (cursoId == Guid.Empty)
                    return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do curso inválido");

                var aulas = await _aulaAppService.ObterPublicadasPorCursoIdAsync(cursoId, includeMateriais);
                return RespostaPadraoApi(data: aulas);
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Cadastra uma nova aula
        /// </summary>
        /// <param name="dto">Dados da aula</param>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<Guid?>), 201)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastroAulaDto dto)
        {
            try
            {
                var command = dto.Adapt<CadastrarAulaCommand>();
                return RespostaPadraoApi(HttpStatusCode.Created, await _mediator.ExecutarComando(command));
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma aula existente
        /// </summary>
        /// <param name="id">ID da aula</param>
        /// <param name="dto">Dados atualizados da aula</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseResult<bool?>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarAulaDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID da aula não confere");

                var command = dto.Adapt<AtualizarAulaCommand>();
                return RespostaPadraoApi<bool?>(await _mediator.ExecutarComando(command));
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Exclui uma aula
        /// </summary>
        /// <param name="id">ID da aula</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                var command = new ExcluirAulaCommand(id);
                return RespostaPadraoApi<bool>(await _mediator.ExecutarComando(command));
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Publica uma aula
        /// </summary>
        /// <param name="id">ID da aula</param>
        [HttpPost("{id}/publicar")]
        [ProducesResponseType(typeof(ResponseResult<bool?>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Publicar(Guid id)
        {
            try
            {
                var command = new PublicarAulaCommand(id);
                return RespostaPadraoApi<bool?>(await _mediator.ExecutarComando(command));
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Despublica uma aula
        /// </summary>
        /// <param name="id">ID da aula</param>
        [HttpPost("{id}/despublicar")]
        [ProducesResponseType(typeof(ResponseResult<bool?>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Despublicar(Guid id)
        {
            try
            {
                var command = new DespublicarAulaCommand(id);
                return RespostaPadraoApi<bool?>(await _mediator.ExecutarComando(command));
            }
            catch (Exception ex)
            {
                return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
