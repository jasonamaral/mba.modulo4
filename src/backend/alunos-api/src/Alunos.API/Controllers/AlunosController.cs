using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Alunos.Application.DTOs;
using Alunos.Application.Interfaces.Services;

namespace Alunos.API.Controllers;

/// <summary>
/// Controller para operações de Aluno
/// </summary>
[ApiController]
[Route("api/alunos")]
[Authorize]
public class AlunosController : ControllerBase
{
    private readonly IAlunoAppService _alunoAppService;
    private readonly ILogger<AlunosController> _logger;

    /// <summary>
    /// Construtor do controller
    /// </summary>
    /// <param name="alunoAppService">Serviço de aplicação de aluno</param>
    /// <param name="logger">Logger</param>
    public AlunosController(
        IAlunoAppService alunoAppService,
        ILogger<AlunosController> logger)
    {
        _alunoAppService = alunoAppService;
        _logger = logger;
    }

    #region Operações de Consulta (Query)

    /// <summary>
    /// Listar todos os alunos
    /// </summary>
    /// <param name="pagina">Número da página</param>
    /// <param name="tamanhoPagina">Tamanho da página</param>
    /// <param name="filtro">Filtro de busca</param>
    /// <param name="ordenacao">Campo de ordenação</param>
    /// <param name="direcao">Direção da ordenação (asc/desc)</param>
    /// <returns>Lista paginada de alunos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AlunoResumoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListarAlunos(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 10,
        [FromQuery] string? filtro = null,
        [FromQuery] string? ordenacao = null,
        [FromQuery] string? direcao = "asc")
    {
        try
        {
            var alunos = await _alunoAppService.ListarAlunosAsync(
                pagina, tamanhoPagina, filtro, ordenacao, direcao);

            return Ok(alunos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao listar alunos");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obter aluno por ID
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns>Dados do aluno</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AlunoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterAlunoPorId(Guid id)
    {
        try
        {
            var aluno = await _alunoAppService.ObterAlunoPorIdAsync(id);

            if (aluno == null)
            {
                return NotFound(new { message = "Aluno não encontrado" });
            }

            return Ok(aluno);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao obter aluno {AlunoId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obter aluno por código de usuário de autenticação
    /// </summary>
    /// <param name="codigoUsuario">Código do usuário de autenticação</param>
    /// <returns>Dados do aluno</returns>
    [HttpGet("usuario/{codigoUsuario:guid}")]
    [ProducesResponseType(typeof(AlunoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterAlunoPorCodigoUsuario(Guid codigoUsuario)
    {
        try
        {
            var aluno = await _alunoAppService.ObterAlunoPorCodigoUsuarioAsync(codigoUsuario);

            if (aluno == null)
            {
                return NotFound(new { message = "Aluno não encontrado" });
            }

            return Ok(aluno);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao obter aluno por código de usuário {CodigoUsuario}", codigoUsuario);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obter perfil completo do aluno
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns>Perfil completo do aluno</returns>
    [HttpGet("{id:guid}/perfil")]
    [ProducesResponseType(typeof(AlunoPerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterPerfilAluno(Guid id)
    {
        try
        {
            var perfil = await _alunoAppService.ObterPerfilAlunoAsync(id);

            if (perfil == null)
            {
                return NotFound(new { message = "Aluno não encontrado" });
            }

            return Ok(perfil);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao obter perfil do aluno {AlunoId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obter dashboard do aluno
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns>Dashboard do aluno</returns>
    [HttpGet("{id:guid}/dashboard")]
    [ProducesResponseType(typeof(AlunoDashboardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterDashboardAluno(Guid id)
    {
        try
        {
            var dashboard = await _alunoAppService.ObterDashboardAlunoAsync(id);

            if (dashboard == null)
            {
                return NotFound(new { message = "Aluno não encontrado" });
            }

            return Ok(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao obter dashboard do aluno {AlunoId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Verificar se aluno existe
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns>Resultado da verificação</returns>
    [HttpHead("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> VerificarExistenciaAluno(Guid id)
    {
        try
        {
            var existe = await _alunoAppService.VerificarExistenciaAlunoAsync(id);

            if (!existe)
            {
                return NotFound();
            }

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao verificar existência do aluno {AlunoId}", id);
            return StatusCode(500);
        }
    }

    #endregion

    #region Operações de Comando (Command)

    /// <summary>
    /// Cadastrar novo aluno
    /// </summary>
    /// <param name="dto">Dados do aluno</param>
    /// <returns>Aluno criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AlunoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CadastrarAluno([FromBody] AlunoCadastroDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var aluno = await _alunoAppService.CadastrarAlunoAsync(dto);

            return CreatedAtAction(nameof(ObterAlunoPorId), new { id = aluno.Id }, aluno);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Erro de validação ao cadastrar aluno: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao cadastrar aluno");
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Atualizar dados do aluno
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <param name="dto">Dados para atualização</param>
    /// <returns>Aluno atualizado</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AlunoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtualizarAluno(Guid id, [FromBody] AlunoAtualizarDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var aluno = await _alunoAppService.AtualizarAlunoAsync(id, dto);

            if (aluno == null)
            {
                return NotFound(new { message = "Aluno não encontrado" });
            }

            return Ok(aluno);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Erro de validação ao atualizar aluno {AlunoId}: {Message}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao atualizar aluno {AlunoId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Ativar aluno
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns>Resultado da operação</returns>
    [HttpPatch("{id:guid}/ativar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtivarAluno(Guid id)
    {
        try
        {
            var resultado = await _alunoAppService.AtivarAlunoAsync(id);

            if (!resultado)
            {
                return NotFound(new { message = "Aluno não encontrado" });
            }

            return Ok(new { message = "Aluno ativado com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao ativar aluno {AlunoId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Desativar aluno
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns>Resultado da operação</returns>
    [HttpPatch("{id:guid}/desativar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DesativarAluno(Guid id)
    {
        try
        {
            var resultado = await _alunoAppService.DesativarAlunoAsync(id);

            if (!resultado)
            {
                return NotFound(new { message = "Aluno não encontrado" });
            }

            return Ok(new { message = "Aluno desativado com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao desativar aluno {AlunoId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Excluir aluno
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns>Resultado da operação</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExcluirAluno(Guid id)
    {
        try
        {
            var resultado = await _alunoAppService.ExcluirAlunoAsync(id);

            if (!resultado)
            {
                return NotFound(new { message = "Aluno não encontrado" });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Erro de operação ao excluir aluno {AlunoId}: {Message}", id, ex.Message);
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao excluir aluno {AlunoId}", id);
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    #endregion
} 