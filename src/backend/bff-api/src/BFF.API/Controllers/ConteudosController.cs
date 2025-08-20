using BFF.API.Models.Request;
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
using BFF.API.Services.Conteudos;

namespace BFF.API.Controllers;

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
    [ProducesResponseType(typeof(ResponseResult<CursoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterCurso([FromRoute] Guid cursoId, [FromQuery] bool includeAulas = false)
    {
        if (cursoId == Guid.Empty)
        {
            return ProcessarErro(HttpStatusCode.BadRequest, "Id do curso inválido.");
        }
        var cacheKey = $"Curso_{cursoId}_IncludeAulas_{includeAulas}";
        var cachedCurso = await _cacheService.GetAsync<ResponseResult<CursoDto>>(cacheKey);

        if (cachedCurso != null)
        {
            return RespostaPadraoApi(HttpStatusCode.OK, cachedCurso, "Curso obtido do cache com sucesso");
        }
        
        var resultado = await _conteudoService.ObterCursoPorIdAsync(cursoId, includeAulas);

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
    [ProducesResponseType(typeof(ResponseResult<PagedResult<CursoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterTodosCursos([FromQuery] CursoFilter filter)
    {
        var cacheKey = $"TodosCursos_Filtro:{JsonSerializer.Serialize(filter)}";
        var cachedCursos = await _cacheService.GetAsync<ResponseResult<PagedResult<CursoDto>>>(cacheKey);

        if (cachedCursos != null)
            return Ok(cachedCursos);

        var resultado = await _conteudoService.ObterTodosCursosAsync(filter);

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
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<CursoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterCursosPorCategoria([FromRoute] Guid categoriaId, [FromQuery] bool includeAulas = false)
    {
        var resultado = await _conteudoService.ObterPorCategoriaIdAsync(categoriaId, includeAulas);

        if (resultado?.Status == (int)HttpStatusCode.OK)
            return Ok(resultado);

        return BadRequest(resultado);
    }

    /// <summary>
    /// Obtém todas as categorias
    /// </summary>
    /// <returns>Lista categorias</returns>
    [HttpGet("categorias")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<CategoriaDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterTodasCategorias()
    {
        var resultado = await _conteudoService.ObterTodasCategoriasAsync();

        if (resultado?.Status == (int)HttpStatusCode.OK)
            return Ok(resultado);

        return BadRequest(resultado);
    }

    /// <summary>
    /// Cadastrar um novo curso
    /// </summary>
    [HttpPost("cursos")]
    [ProducesResponseType(typeof(ResponseResult<Guid?>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AdicionarCurso([FromBody] CursoCriarRequest curso)
    {
        var response = await _conteudoService.AdicionarCursoAsync(curso);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        await _cacheService.RemovePatternAsync("TodosCursos_Filtro:");

        return StatusCode(response?.Status ?? 500, response);
    }

    /// <summary>
    /// Atualizar um curso existente
    /// </summary>
    [HttpPut("cursos/{cursoId}")]
    [ProducesResponseType(typeof(ResponseResult<CursoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AtualizarCurso(Guid cursoId, [FromBody] AtualizarCursoRequest curso)
    {
        var response = await _conteudoService.AtualizarCursoAsync(cursoId, curso);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        await _cacheService.RemovePatternAsync("TodosCursos_Filtro:");
        return Ok(response);
    }

    /// <summary>
    /// Excluir um curso
    /// </summary>
    [HttpDelete("cursos/{cursoId}")]
    [ProducesResponseType(typeof(ResponseResult<bool?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ExcluirCurso(Guid cursoId)
    {
        var response = await _conteudoService.ExcluirCursoAsync(cursoId);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        await _cacheService.RemovePatternAsync("TodosCursos_Filtro:");
        return Ok(response);
    }

    /// <summary>
    /// Obter Aulas por Curso ID
    /// </summary>
    [HttpGet("cursos/{cursoId}/aulas")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterAulasPorCursoId([FromRoute] Guid cursoId)
    {
        var resultado = await _conteudoService.ObterCursoPorIdAsync(cursoId, true);
        if (resultado?.Status == (int)HttpStatusCode.OK)
            return Ok(resultado);
        return BadRequest(resultado);
    }


    /// <summary>
    /// Obter Conteudo Programatico por Curso ID
    /// </summary>
    [HttpGet("cursos/{cursoId}/conteudo-programatico")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Usuario, Administrador")]
    public async Task<IActionResult> ObterConteudoProgramaticoPorCursoId([FromRoute] Guid cursoId)
    {
        var resultado = await _conteudoService.ObterConteudoProgramaticoPorCursoIdAsync(cursoId);
        if (resultado?.Status == (int)HttpStatusCode.OK)
            return Ok(resultado);
        return BadRequest(resultado);
    }

    /// <summary>
    /// Cadastrar uma nova aula em um curso
    /// </summary>
    [HttpPost("cursos/{cursoId}/aulas")]
    [ProducesResponseType(typeof(ResponseResult<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AdicionarAula( [FromRoute] Guid cursoId,
                                                    [FromBody] AulaCriarRequest request)
    {
        var response = await _conteudoService.AdicionarAulaAsync(cursoId, request);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        await _cacheService.RemovePatternAsync($"Curso_{cursoId}_IncludeAulas_*");

        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Atualizar uma aula existente
    /// </summary>
    [HttpPut("cursos/{cursoId}/aulas/{aulaId}")]
    [ProducesResponseType(typeof(ResponseResult<AulaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AtualizarAula(
        [FromRoute] Guid cursoId,
        [FromRoute] Guid aulaId,
        [FromBody] AulaAtualizarRequest request)
    {
        if (aulaId != request.Id)
            return ProcessarErro(HttpStatusCode.BadRequest, "ID da aula na rota não confere com o corpo.");

        var response = await _conteudoService.AtualizarAulaAsync(cursoId, request);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        await _cacheService.RemovePatternAsync($"Curso_{cursoId}_IncludeAulas_*");

        return Ok(response);
    }

    /// <summary>
    /// Excluir uma aula
    /// </summary>
    [HttpDelete("cursos/{cursoId}/aulas/{aulaId}")]
    [ProducesResponseType(typeof(ResponseResult<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseResult<string>), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ExcluirAula(
        [FromRoute] Guid cursoId,
        [FromRoute] Guid aulaId)
    {
        var response = await _conteudoService.ExcluirAulaAsync(cursoId, aulaId);

        if (response?.Status == (int)HttpStatusCode.BadRequest)
            return BadRequest(response);

        await _cacheService.RemovePatternAsync($"Curso_{cursoId}_IncludeAulas_*");

        return Ok(response);
    }
}
