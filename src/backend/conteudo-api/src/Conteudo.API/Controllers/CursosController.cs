using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Conteudo.Application.Interfaces.Services;
using Conteudo.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Conteudo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class CursosController : ControllerBase
{
    private readonly ICursoAppService _cursoAppService;

    public CursosController(ICursoAppService cursoAppService)
    {
        _cursoAppService = cursoAppService;
    }

    /// <summary>
    /// Obtém todos os cursos
    /// </summary>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Lista de cursos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CursoDto>), 200)]
    [ProducesResponseType(typeof(ApiError), 400)]
    public async Task<IActionResult> GetCursos([FromQuery] bool includeAulas = false)
    {
        try
        {
            var cursos = await _cursoAppService.GetAllAsync(includeAulas);
            return Ok(cursos);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
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
    [ProducesResponseType(typeof(ApiError), 404)]
    public async Task<IActionResult> GetCurso(
        [FromRoute] Guid id, 
        [FromQuery] bool includeAulas = false)
    {
        try
        {
            var curso = await _cursoAppService.GetByIdAsync(id, includeAulas);
            if (curso == null)
                return NotFound(new ApiError { Message = "Curso não encontrado" });

            return Ok(curso);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
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
    [ProducesResponseType(typeof(ApiError), 400)]
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
            return BadRequest(new ApiError { Message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém cursos ativos
    /// </summary>
    /// <param name="includeAulas">Se deve incluir aulas na resposta</param>
    /// <returns>Lista de cursos ativos</returns>
    [HttpGet("ativos")]
    [ProducesResponseType(typeof(IEnumerable<CursoDto>), 200)]
    [ProducesResponseType(typeof(ApiError), 400)]
    public async Task<IActionResult> GetCursosAtivos([FromQuery] bool includeAulas = false)
    {
        try
        {
            var cursos = await _cursoAppService.GetAtivosAsync(includeAulas);
            return Ok(cursos);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
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
    [ProducesResponseType(typeof(ApiError), 400)]
    public async Task<IActionResult> BuscarCursos(
        [FromQuery] [Required] string searchTerm,
        [FromQuery] bool includeAulas = false)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest(new ApiError { Message = "Termo de busca é obrigatório" });

            var cursos = await _cursoAppService.SearchAsync(searchTerm, includeAulas);
            return Ok(cursos);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
        }
    }

    /// <summary>
    /// Cadastra um novo curso
    /// </summary>
    /// <param name="dto">Dados do curso</param>
    /// <returns>ID do curso criado</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CursoCreatedResponse), 201)]
    [ProducesResponseType(typeof(ApiError), 400)]
    public async Task<IActionResult> CadastrarCurso([FromBody] CadastroCursoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError 
                { 
                    Message = "Dados inválidos",
                    Details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var cursoId = await _cursoAppService.CadastrarCursoAsync(dto);
            return CreatedAtAction(nameof(GetCurso), new { id = cursoId }, new CursoCreatedResponse { Id = cursoId });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um curso existente
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <param name="dto">Dados atualizados do curso</param>
    /// <returns>Dados do curso atualizado</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CursoDto), 200)]
    [ProducesResponseType(typeof(ApiError), 400)]
    [ProducesResponseType(typeof(ApiError), 404)]
    public async Task<IActionResult> AtualizarCurso([FromRoute] Guid id, [FromBody] AtualizarCursoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiError 
                { 
                    Message = "Dados inválidos",
                    Details = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            if (id != dto.Id)
                return BadRequest(new ApiError { Message = "ID do curso não confere" });

            var curso = await _cursoAppService.AtualizarCursoAsync(dto);
            return Ok(curso);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new ApiError { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
        }
    }

    /// <summary>
    /// Ativa um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Confirmação da ativação</returns>
    [HttpPatch("{id}/ativar")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ApiError), 404)]
    public async Task<IActionResult> AtivarCurso([FromRoute] Guid id)
    {
        try
        {
            await _cursoAppService.AtivarCursoAsync(id);
            return Ok(new ApiSuccess { Message = "Curso ativado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new ApiError { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
        }
    }

    /// <summary>
    /// Desativa um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Confirmação da desativação</returns>
    [HttpPatch("{id}/desativar")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ApiError), 404)]
    public async Task<IActionResult> DesativarCurso([FromRoute] Guid id)
    {
        try
        {
            await _cursoAppService.DesativarCursoAsync(id);
            return Ok(new ApiSuccess { Message = "Curso desativado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new ApiError { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
        }
    }

    /// <summary>
    /// Exclui um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Confirmação da exclusão</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ApiError), 404)]
    public async Task<IActionResult> ExcluirCurso([FromRoute] Guid id)
    {
        try
        {
            await _cursoAppService.ExcluirCursoAsync(id);
            return Ok(new ApiSuccess { Message = "Curso excluído com sucesso" });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new ApiError { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém as aulas de um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Lista de aulas do curso</returns>
    [HttpGet("{id}/aulas")]
    [ProducesResponseType(typeof(IEnumerable<AulaDto>), 200)]
    [ProducesResponseType(typeof(ApiError), 404)]
    public async Task<IActionResult> GetAulasDoCurso([FromRoute] Guid id)
    {
        try
        {
            var curso = await _cursoAppService.GetByIdAsync(id, includeAulas: true);
            if (curso == null)
                return NotFound(new ApiError { Message = "Curso não encontrado" });

            return Ok(curso.Aulas);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém o conteúdo programático de um curso
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Conteúdo programático</returns>
    [HttpGet("{id}/conteudo-programatico")]
    [ProducesResponseType(typeof(ConteudoProgramaticoDto), 200)]
    [ProducesResponseType(typeof(ApiError), 404)]
    public async Task<IActionResult> GetConteudoProgramatico([FromRoute] Guid id)
    {
        try
        {
            var curso = await _cursoAppService.GetByIdAsync(id);
            if (curso == null)
                return NotFound(new ApiError { Message = "Curso não encontrado" });

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
            return BadRequest(new ApiError { Message = ex.Message });
        }
    }

    /// <summary>
    /// Registra acesso ao curso para auditoria
    /// </summary>
    /// <param name="id">ID do curso</param>
    /// <returns>Confirmação do registro</returns>
    [HttpPost("{id}/acesso")]
    [ProducesResponseType(typeof(ApiSuccess), 200)]
    [ProducesResponseType(typeof(ApiError), 404)]
    public async Task<IActionResult> RegistrarAcesso([FromRoute] Guid id)
    {
        try
        {
            var curso = await _cursoAppService.GetByIdAsync(id);
            if (curso == null)
                return NotFound(new ApiError { Message = "Curso não encontrado" });

            // TODO: Implementar auditoria de acesso
            // await _auditService.RegistrarAcessoCurso(id, User.Identity.Name);

            return Ok(new ApiSuccess { Message = "Acesso registrado com sucesso" });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiError { Message = ex.Message });
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

public class ApiError
{
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string>? Details { get; set; }
}

public class ApiSuccess
{
    public string Message { get; set; } = string.Empty;
}

#endregion 