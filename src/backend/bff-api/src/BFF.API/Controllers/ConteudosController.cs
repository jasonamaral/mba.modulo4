using BFF.API.Models.Request;
using BFF.API.Services;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using Core.Communication;
using Core.Communication.Filters;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using MediatR;

namespace BFF.API.Controllers
{
    /// <summary>
    /// Controller de Conteudos no BFF - Orquestra chamadas para Conteudo.API
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ConteudosController : BffController
    {
        private readonly IConteudoService _conteudoService;
        private readonly ICacheService _cacheService;

        public ConteudosController(IMediatorHandler mediator,
                                 INotificationHandler<DomainNotificacaoRaiz> notifications,
                                 INotificador notificador,
                                 IConteudoService conteudoService,
                                 ICacheService cacheService) : base(mediator, notifications, notificador)
        {
            _conteudoService = conteudoService;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Obtém um curso por ID
        /// </summary>
        /// <param name="cursoId">ID do curso</param>
        /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
        /// <returns>Dados do curso</returns>
        [HttpGet("{cursoId}")]
        [ProducesResponseType(typeof(ResponseResult<CursoDto>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        public async Task<IActionResult> ObterCurso([FromRoute] Guid cursoId, [FromQuery] bool includeAulas = false)
        {
            if (cursoId == Guid.Empty)
            {
                return ProcessarErro(System.Net.HttpStatusCode.BadRequest, "Id do curso inválido.");
            }
            var cacheKey = $"Curso_{cursoId}_IncludeAulas_{includeAulas}";
            var cachedCurso = await _cacheService.GetAsync<ResponseResult<CursoDto>>(cacheKey);

            if (cachedCurso != null)
            {
                return RespostaPadraoApi(System.Net.HttpStatusCode.OK, cachedCurso, "Curso obtido do cache com sucesso");
            }

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var resultado = await _conteudoService.ObterCursoPorId(cursoId, token);

            if (resultado?.Status == (int)HttpStatusCode.OK)
            {
                await _cacheService.SetAsync(cacheKey, resultado, TimeSpan.FromMinutes(30));
                return Ok(resultado);
            }

            return BadRequest(resultado);
        }

        /// <summary>
        /// Obter todos os cursos
        /// </summary>
        /// <param name="filter">Filtro para paginação e busca</param>
        [HttpGet("cursos")]
        [ProducesResponseType(typeof(ResponseResult<PagedResult<CursoDto>>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        public async Task<IActionResult> ObterTodosCursos([FromQuery] CursoFilter filter)
        {
            var cacheKey = $"TodosCursos_Filtro:{JsonSerializer.Serialize(filter)}";
            var cachedCursos = await _cacheService.GetAsync<ResponseResult<PagedResult<CursoDto>>>(cacheKey);

            if (cachedCursos != null)
                return Ok(cachedCursos);

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            var resultado = await _conteudoService.ObterTodosCursos(token, filter);

            if (resultado?.Status == (int)HttpStatusCode.OK)
            {
                await _cacheService.SetAsync(cacheKey, resultado, TimeSpan.FromMinutes(30));
                return Ok(resultado);
            }
            return BadRequest(resultado);
        }

        /// <summary>
        /// Obtém cursos por categoria
        /// </summary>
        /// <param name="categoriaId">ID da categoria</param>
        /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
        /// <returns>Lista de cursos da categoria</returns>
        [HttpGet("categoria/{categoriaId}")]
        [ProducesResponseType(typeof(ResponseResult<IEnumerable<CursoDto>>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        public async Task<IActionResult> ObterCursosPorCategoria([FromRoute] Guid categoriaId, [FromQuery] bool includeAulas = false)
        {
            try
            {   
                var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var resultado = await _conteudoService.ObterPorCategoriaIdAsync(token, categoriaId, includeAulas);

                if (resultado?.Status == (int)HttpStatusCode.OK)
                {
                    return Ok(resultado);
                }

                return BadRequest(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cadastrar um novo curso
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpPost("cursos")]
        [ProducesResponseType(typeof(ResponseResult<Guid>), 201)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        public async Task<IActionResult> AdicionarCurso([FromBody] CursoCriarRequest curso)
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            var response = await _conteudoService.AdicionarCurso(curso, token);

            if (response?.Status == (int)HttpStatusCode.BadRequest)
                return BadRequest(response);

            return StatusCode(response?.Status ?? 500, response);
        }

        /// <summary>
        /// Atualizar um curso existente
        /// </summary>
        [Authorize(Roles = "Administrador")]
        [HttpPut("cursos/{cursoId}")]
        [ProducesResponseType(typeof(ResponseResult<CursoDto>), 200)]
        [ProducesResponseType(typeof(ResponseResult<string>), 400)]
        [ProducesResponseType(typeof(ResponseResult<string>), 404)]
        public async Task<IActionResult> AtualizarCurso(Guid cursoId, [FromBody] AtualizarCursoRequest curso)
        {
            if (cursoId == Guid.Empty)
                return BadRequest(new ResponseResult<string>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Errors = new ResponseErrorMessages
                    {
                        Mensagens = new List<string> { "Id do curso inválido." }
                    }
                });

            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            var response = await _conteudoService.AtualizarCurso(cursoId, curso, token);

            if (response?.Status == (int)HttpStatusCode.NotFound)
                return NotFound(response);

            if (response?.Status == (int)HttpStatusCode.BadRequest)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
