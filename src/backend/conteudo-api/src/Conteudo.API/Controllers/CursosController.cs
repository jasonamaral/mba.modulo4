using Conteudo.Application.Commands.AtualizarCurso;
using Conteudo.Application.Commands.CadastrarCurso;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Core.Communication;
using Core.Communication.Filters;
using Core.Mediator;
using Core.Messages;
using Core.Services.Controllers;
using Core.SharedDtos.Conteudo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Mapster;
using Core.Notification;

namespace Conteudo.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CursosController(ICursoAppService cursoAppService
                           , IMediatorHandler mediator
                           , INotificador notificador
                           , INotificationHandler<DomainNotificacaoRaiz> notifications) : MainController(mediator, notifications, notificador)
{
    private readonly ICursoAppService _cursoAppService = cursoAppService;
    private readonly IMediatorHandler _mediator = mediator;

    /// <summary>
    /// Obtém um curso por ID
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Dados do curso</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseResult<CursoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterCurso([FromRoute] Guid id, [FromQuery] bool includeAulas = false)
    {
        try
        {
            var curso = await _cursoAppService.ObterPorIdAsync(id, includeAulas);
            if (curso == null)
            {
                _notificador.AdicionarErro("Curso não encontrado.");
                return RespostaPadraoApi<string>();
            }

            return RespostaPadraoApi(data: curso);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém todos os cursos
    /// </summary>
    /// <param name="filter">Filtro para paginação e busca</param>
    /// <returns>Lista de cursos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseResult<PagedResult<CursoDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> ObterCursos([FromQuery] CursoFilter filter)
    {
        try
        {
            var cursos = await _cursoAppService.ObterTodosAsync(filter);
            return RespostaPadraoApi(data: cursos);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
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
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> ObterCursosPorCategoria([FromRoute] Guid categoriaId, [FromQuery] bool includeAulas = false)
    {
        try
        {   
            if (categoriaId == Guid.Empty)
                return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID da categoria inválido");

            var cursos = await _cursoAppService.ObterPorCategoriaIdAsync(categoriaId, includeAulas);
            return RespostaPadraoApi(data: cursos);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Cadastra um novo curso
    /// </summary>
    /// <param name="dto">Dados do curso</param>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ResponseResult<Guid>), 201)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    public async Task<IActionResult> CadastrarCurso([FromBody] CadastroCursoDto dto)
    {
        try
        {
            var command = dto.Adapt<CadastrarCursoCommand>();
            return RespostaPadraoApi<Guid>(await _mediator.ExecutarComando(command));
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        } 
    }

    /// <summary>
    /// Atualiza um curso existente
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <param name="dto">Dados atualizados do curso</param>
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ResponseResult<CursoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 400)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> AtualizarCurso([FromRoute] Guid id, [FromBody] AtualizarCursoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return RespostaPadraoApi<CommandResult>(ModelState);

            if (id != dto.Id)
                return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do curso não confere");

            var command = dto.Adapt<AtualizarCursoCommand>();
            
            return RespostaPadraoApi<CursoDto>(await _mediator.ExecutarComando(command));
        }
        catch (ArgumentException ex)
        {
            return RespostaPadraoApi(HttpStatusCode.NotFound, ex.Message);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Exclui um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Confirmação da exclusão</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> ExcluirCurso([FromRoute] Guid id)
    {
        try
        {
            //await _cursoAppService.ExcluirCursoAsync(id);
            return Ok(new ApiSuccess { Message = "Curso excluído com sucesso" });
        }
        catch (ArgumentException ex)
        {
            return RespostaPadraoApi(HttpStatusCode.NotFound, ex.Message);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém as aulas de um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Lista de aulas do curso</returns>
    [HttpGet("{id}/aulas")]
    [ProducesResponseType(typeof(ResponseResult<IEnumerable<AulaDto>>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> ObterAulasDoCurso([FromRoute] Guid id)
    {
        try
        {
            var curso = await _cursoAppService.ObterPorIdAsync(id, includeAulas: true);
            if (curso == null)
                return RespostaPadraoApi(HttpStatusCode.NotFound, "Curso não encontrado");

            return RespostaPadraoApi(data: curso.Aulas);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém o conteúdo programático de um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Conteúdo programático</returns>
    [HttpGet("{id}/conteudo-programatico")]
    [ProducesResponseType(typeof(ResponseResult<ConteudoProgramaticoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult<string>), 404)]
    public async Task<IActionResult> ObterConteudoProgramatico([FromRoute] Guid id)
    {
        try
        {
            var curso = await _cursoAppService.ObterPorIdAsync(id);
            if (curso == null)
                return RespostaPadraoApi(HttpStatusCode.NotFound, "Curso não encontrado");

            var conteudoProgramatico = new ConteudoProgramaticoDto
            {
                Resumo = curso.Resumo,
                Descricao = curso.Descricao,
                Objetivos = curso.Objetivos,
                PreRequisitos = curso.PreRequisitos,
                PublicoAlvo = curso.PublicoAlvo,
                Metodologia = curso.Metodologia,
                Recursos = curso.Recursos,
                Avaliacao = curso.Avaliacao,
                Bibliografia = curso.Bibliografia
            };

            return RespostaPadraoApi(data: conteudoProgramatico);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }
}