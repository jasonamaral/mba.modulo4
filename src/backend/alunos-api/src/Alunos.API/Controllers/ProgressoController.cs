using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Alunos.Application.DTOs;
using Alunos.Application.Interfaces.Services;

namespace Alunos.API.Controllers
{
    /// <summary>
    /// Controller para operações de Progresso
    /// </summary>
    [ApiController]
    [Route("api/progresso")]
    [Authorize]
    public class ProgressoController : ControllerBase
    {
        private readonly IProgressoAppService _progressoAppService;
        private readonly ILogger<ProgressoController> _logger;

        /// <summary>
        /// Construtor do controller
        /// </summary>
        /// <param name="progressoAppService">Serviço de aplicação de progresso</param>
        /// <param name="logger">Logger</param>
        public ProgressoController(
            IProgressoAppService progressoAppService,
            ILogger<ProgressoController> logger)
        {
            _progressoAppService = progressoAppService;
            _logger = logger;
        }

        /// <summary>
        /// Obter progresso por ID
        /// </summary>
        /// <param name="id">ID do progresso</param>
        /// <returns>Dados do progresso</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProgressoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterProgressoPorId(Guid id)
        {
            try
            {
                var progresso = await _progressoAppService.ObterProgressoPorIdAsync(id);

                if (progresso == null)
                {
                    _logger.LogWarning("Progresso não encontrado. ID: {ProgressoId}", id);
                    return NotFound(new { message = "Progresso não encontrado" });
                }

                _logger.LogInformation("Progresso obtido com sucesso. ID: {ProgressoId}", id);

                return Ok(progresso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter progresso {ProgressoId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter progresso por matrícula
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <returns>Lista de progresso da matrícula</returns>
        [HttpGet("matricula/{matriculaId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<ProgressoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterProgressoPorMatricula(Guid matriculaId)
        {
            try
            {
                var progresso = await _progressoAppService.ObterProgressoPorMatriculaAsync(matriculaId);

                _logger.LogInformation("Progresso da matrícula obtido com sucesso. Matrícula: {MatriculaId}", matriculaId);

                return Ok(progresso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter progresso da matrícula {MatriculaId}", matriculaId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter progresso por matrícula e aula
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        /// <returns>Progresso da aula</returns>
        [HttpGet("matricula/{matriculaId:guid}/aula/{aulaId:guid}")]
        [ProducesResponseType(typeof(ProgressoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterProgressoPorMatriculaEAula(Guid matriculaId, Guid aulaId)
        {
            try
            {
                var progresso = await _progressoAppService.ObterProgressoPorMatriculaEAulaAsync(matriculaId, aulaId);

                if (progresso == null)
                {
                    _logger.LogWarning("Progresso não encontrado. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                        matriculaId, aulaId);
                    return NotFound(new { message = "Progresso não encontrado" });
                }

                _logger.LogInformation("Progresso da aula obtido com sucesso. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                    matriculaId, aulaId);

                return Ok(progresso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter progresso da matrícula {MatriculaId} e aula {AulaId}", 
                    matriculaId, aulaId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Atualizar progresso
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        /// <param name="dto">Dados de atualização do progresso</param>
        /// <returns>Progresso atualizado</returns>
        [HttpPut("matricula/{matriculaId:guid}/aula/{aulaId:guid}")]
        [ProducesResponseType(typeof(ProgressoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarProgresso(
            Guid matriculaId, 
            Guid aulaId, 
            [FromBody] ProgressoAtualizarDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var progresso = await _progressoAppService.AtualizarProgressoAsync(matriculaId, aulaId, dto);

                if (progresso == null)
                {
                    _logger.LogWarning("Progresso não encontrado para atualização. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                        matriculaId, aulaId);
                    return NotFound(new { message = "Progresso não encontrado" });
                }

                _logger.LogInformation("Progresso atualizado com sucesso. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                    matriculaId, aulaId);

                return Ok(progresso);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao atualizar progresso. Matrícula: {MatriculaId}, Aula: {AulaId}: {Message}", 
                    matriculaId, aulaId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao atualizar progresso. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                    matriculaId, aulaId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Concluir aula
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        /// <param name="dto">Dados de conclusão</param>
        /// <returns>Progresso com aula concluída</returns>
        [HttpPatch("matricula/{matriculaId:guid}/aula/{aulaId:guid}/concluir")]
        [ProducesResponseType(typeof(ProgressoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConcluirAula(
            Guid matriculaId, 
            Guid aulaId, 
            [FromBody] ProgressoConclusaoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var progresso = await _progressoAppService.ConcluirAulaAsync(matriculaId, aulaId, dto);

                if (progresso == null)
                {
                    _logger.LogWarning("Progresso não encontrado para conclusão. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                        matriculaId, aulaId);
                    return NotFound(new { message = "Progresso não encontrado" });
                }

                _logger.LogInformation("Aula concluída com sucesso. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                    matriculaId, aulaId);

                return Ok(progresso);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao concluir aula. Matrícula: {MatriculaId}, Aula: {AulaId}: {Message}", 
                    matriculaId, aulaId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao concluir aula. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                    matriculaId, aulaId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Iniciar aula
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        /// <returns>Progresso com aula iniciada</returns>
        [HttpPost("matricula/{matriculaId:guid}/aula/{aulaId:guid}/iniciar")]
        [ProducesResponseType(typeof(ProgressoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IniciarAula(Guid matriculaId, Guid aulaId)
        {
            try
            {
                var progresso = await _progressoAppService.IniciarAulaAsync(matriculaId, aulaId);

                if (progresso == null)
                {
                    _logger.LogWarning("Não foi possível iniciar a aula. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                        matriculaId, aulaId);
                    return NotFound(new { message = "Matrícula ou aula não encontrada" });
                }

                _logger.LogInformation("Aula iniciada com sucesso. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                    matriculaId, aulaId);

                return CreatedAtAction(
                    nameof(ObterProgressoPorMatriculaEAula),
                    new { matriculaId, aulaId },
                    progresso);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao iniciar aula. Matrícula: {MatriculaId}, Aula: {AulaId}: {Message}", 
                    matriculaId, aulaId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao iniciar aula. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                    matriculaId, aulaId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter relatório de progresso
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <returns>Relatório de progresso</returns>
        [HttpGet("matricula/{matriculaId:guid}/relatorio")]
        [ProducesResponseType(typeof(ProgressoRelatorioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterRelatorioProgresso(Guid matriculaId)
        {
            try
            {
                var relatorio = await _progressoAppService.ObterRelatorioProgressoAsync(matriculaId);

                if (relatorio == null)
                {
                    _logger.LogWarning("Relatório de progresso não encontrado. Matrícula: {MatriculaId}", matriculaId);
                    return NotFound(new { message = "Matrícula não encontrada" });
                }

                _logger.LogInformation("Relatório de progresso obtido com sucesso. Matrícula: {MatriculaId}", matriculaId);

                return Ok(relatorio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter relatório de progresso. Matrícula: {MatriculaId}", matriculaId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Abandonar aula
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        /// <returns>Progresso com aula abandonada</returns>
        [HttpPatch("matricula/{matriculaId:guid}/aula/{aulaId:guid}/abandonar")]
        [ProducesResponseType(typeof(ProgressoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AbandonarAula(Guid matriculaId, Guid aulaId)
        {
            try
            {
                var progresso = await _progressoAppService.AbandonarAulaAsync(matriculaId, aulaId);

                if (progresso == null)
                {
                    _logger.LogWarning("Progresso não encontrado para abandono. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                        matriculaId, aulaId);
                    return NotFound(new { message = "Progresso não encontrado" });
                }

                _logger.LogInformation("Aula abandonada com sucesso. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                    matriculaId, aulaId);

                return Ok(progresso);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao abandonar aula. Matrícula: {MatriculaId}, Aula: {AulaId}: {Message}", 
                    matriculaId, aulaId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao abandonar aula. Matrícula: {MatriculaId}, Aula: {AulaId}", 
                    matriculaId, aulaId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }
    }
} 