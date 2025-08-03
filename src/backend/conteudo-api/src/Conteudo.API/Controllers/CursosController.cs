using Conteudo.API.Controllers.Base;
using Conteudo.Application.Commands;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Core.Communication;
using Core.Mediator;
using Core.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Mapster;

namespace Conteudo.API.Controllers;

[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CursosController : MainController
{
    private readonly ICursoAppService _cursoAppService;
    private readonly IMediatorHandler _mediator;

    public CursosController(INotificador notificador
                           , ICursoAppService cursoAppService
                           , IMediatorHandler mediator) : base(notificador)
    {
        _cursoAppService = cursoAppService;
        _mediator = mediator;
    }

    /// <summary>
    /// Obtém todos os cursos
    /// </summary>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Lista de cursos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CursoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult), 400)]
    public async Task<IActionResult> ObterCursos([FromQuery] bool includeAulas = false)
    {
        try
        {
            var cursos = await _cursoAppService.ObterTodosAsync(includeAulas);
            return RespostaPadraoApi(data: cursos);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém um curso por ID
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Dados do curso</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CursoDto), 200)]
    [ProducesResponseType(typeof(ResponseResult), 404)]
    public async Task<IActionResult> ObterCurso(
        [FromRoute] Guid id, 
        [FromQuery] bool includeAulas = false)
    {
        try
        {
            var curso = await _cursoAppService.ObterPorIdAsync(id, includeAulas);
            if (curso == null)
                return RespostaPadraoApi(HttpStatusCode.NotFound, "Curso não encontrado");

            return RespostaPadraoApi(data: curso);
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
    [ProducesResponseType(typeof(IEnumerable<CursoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult), 400)]
    public async Task<IActionResult> GetCursosPorCategoria(
        [FromRoute] Guid categoriaId,
        [FromQuery] bool includeAulas = false)
    {
        try
        {
            var cursos = await _cursoAppService.GetByCategoriaIdAsync(categoriaId, includeAulas);
            return Ok(cursos);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Obtém cursos ativos
    /// </summary>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Lista de cursos ativos</returns>
    [HttpGet("ativos")]
    [ProducesResponseType(typeof(IEnumerable<CursoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult), 400)]
    public async Task<IActionResult> GetCursosAtivos([FromQuery] bool includeAulas = false)
    {
        try
        {
            var cursos = await _cursoAppService.GetAtivosAsync(includeAulas);
            return Ok(cursos);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Busca cursos por termo
    /// </summary>
    /// <param name="searchTerm">Termo de busca</param>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Lista de cursos encontrados</returns>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<CursoDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult), 400)]
    public async Task<IActionResult> BuscarCursos(
        [FromQuery] [Required] string searchTerm,
        [FromQuery] bool includeAulas = false)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return RespostaPadraoApi(HttpStatusCode.BadRequest, "Termo de busca é obrigatório");

            var cursos = await _cursoAppService.SearchAsync(searchTerm, includeAulas);
            return Ok(cursos);
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
    [ProducesResponseType(typeof(ResponseResult), 201)]
    [ProducesResponseType(typeof(ResponseResult), 400)]
    public async Task<IActionResult> CadastrarCurso([FromBody] CadastroCursoDto dto)
    {
        try
        {
            var command = dto.Adapt<CadastrarCursoCommand>();
            return RespostaPadraoApi(await _mediator.ExecutarComando(command));
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
    [ProducesResponseType(typeof(ResponseResult), 200)]
    [ProducesResponseType(typeof(ResponseResult), 400)]
    [ProducesResponseType(typeof(ResponseResult), 404)]
    public async Task<IActionResult> AtualizarCurso([FromRoute] Guid id, [FromBody] AtualizarCursoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return RespostaPadraoApi(ModelState);

            if (id != dto.Id)
                return RespostaPadraoApi(HttpStatusCode.BadRequest, "ID do curso não confere");

            var command = dto.Adapt<AtualizarCursoCommand>();
            
            return RespostaPadraoApi(await _mediator.ExecutarComando(command));
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
    /// Ativa um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Confirmação da ativação</returns>
    [HttpPatch("{id}/ativar")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ResponseResult), 404)]
    public async Task<IActionResult> AtivarCurso([FromRoute] Guid id)
    {
        try
        {
            //await _cursoAppService.AtivarCursoAsync(id);
            return Ok(new ApiSuccess { Message = "Curso ativado com sucesso" });
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
    /// Desativa um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Confirmação da desativação</returns>
    [HttpPatch("{id}/desativar")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ResponseResult), 404)]
    public async Task<IActionResult> DesativarCurso([FromRoute] Guid id)
    {
        try
        {
            //await _cursoAppService.DesativarCursoAsync(id);
            return Ok(new ApiSuccess { Message = "Curso desativado com sucesso" });
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
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ResponseResult), 404)]
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
    [ProducesResponseType(typeof(IEnumerable<AulaDto>), 200)]
    [ProducesResponseType(typeof(ResponseResult), 404)]
    public async Task<IActionResult> GetAulasDoCurso([FromRoute] Guid id)
    {
        try
        {
            var curso = await _cursoAppService.ObterPorIdAsync(id, includeAulas: true);
            if (curso == null)
                return RespostaPadraoApi(HttpStatusCode.NotFound, "Curso não encontrado");

            return Ok(curso.Aulas);
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
    [ProducesResponseType(typeof(ConteudoProgramaticoDto), 200)]
    [ProducesResponseType(typeof(ResponseResult), 404)]
    public async Task<IActionResult> GetConteudoProgramatico([FromRoute] Guid id)
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

            return Ok(conteudoProgramatico);
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    /// <summary>
    /// Registra acesso ao curso para auditoria
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Confirmação do registro</returns>
    [HttpPost("{id}/acesso")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ResponseResult), 404)]
    public async Task<IActionResult> RegistrarAcesso([FromRoute] Guid id)
    {
        try
        {
            var curso = await _cursoAppService.ObterPorIdAsync(id);
            if (curso == null)
                return RespostaPadraoApi(HttpStatusCode.NotFound, "Curso não encontrado");

            // TODO: Implementar auditoria de acesso
            // await _auditService.RegistrarAcessoCurso(id, User.Identity.Name);

            return Ok(new ApiSuccess { Message = "Acesso registrado com sucesso" });
        }
        catch (Exception ex)
        {
            return RespostaPadraoApi(HttpStatusCode.BadRequest, ex.Message);
        }
    }
}

#region Response DTOs

public class CursoCreatedResponse
{
    public Guid Id { get; set; }
}

public class ConteudoProgramaticoDto
{
    public string Resumo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Objetivos { get; set; } = string.Empty;
    public string PreRequisitos { get; set; } = string.Empty;
    public string PublicoAlvo { get; set; } = string.Empty;
    public string Metodologia { get; set; } = string.Empty;
    public string Recursos { get; set; } = string.Empty;
    public string Avaliacao { get; set; } = string.Empty;
    public string Bibliografia { get; set; } = string.Empty;
}

#endregion 