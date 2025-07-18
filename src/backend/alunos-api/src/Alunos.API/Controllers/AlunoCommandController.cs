using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Alunos.Application.DTOs;
using Alunos.Application.Interfaces.Services;

namespace Alunos.API.Controllers
{
    /// <summary>
    /// Controller para operações de comando (escrita) de Aluno
    /// </summary>
    [ApiController]
    [Route("api/alunos")]
    [Authorize]
    public class AlunoCommandController : ControllerBase
    {
        private readonly IAlunoAppService _alunoAppService;
        private readonly ILogger<AlunoCommandController> _logger;

        /// <summary>
        /// Construtor do controller
        /// </summary>
        /// <param name="alunoAppService">Serviço de aplicação de aluno</param>
        /// <param name="logger">Logger</param>
        public AlunoCommandController(
            IAlunoAppService alunoAppService,
            ILogger<AlunoCommandController> logger)
        {
            _alunoAppService = alunoAppService;
            _logger = logger;
        }

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
                
                _logger.LogInformation("Aluno cadastrado com sucesso. ID: {AlunoId}", aluno.Id);
                
                return CreatedAtAction(
                    nameof(AlunoQueryController.ObterAlunoPorId),
                    "AlunoQuery",
                    new { id = aluno.Id },
                    aluno);
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
                    _logger.LogWarning("Aluno não encontrado para atualização. ID: {AlunoId}", id);
                    return NotFound(new { message = "Aluno não encontrado" });
                }

                _logger.LogInformation("Aluno atualizado com sucesso. ID: {AlunoId}", id);
                
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
                    _logger.LogWarning("Aluno não encontrado para ativação. ID: {AlunoId}", id);
                    return NotFound(new { message = "Aluno não encontrado" });
                }

                _logger.LogInformation("Aluno ativado com sucesso. ID: {AlunoId}", id);
                
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
                    _logger.LogWarning("Aluno não encontrado para desativação. ID: {AlunoId}", id);
                    return NotFound(new { message = "Aluno não encontrado" });
                }

                _logger.LogInformation("Aluno desativado com sucesso. ID: {AlunoId}", id);
                
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
                    _logger.LogWarning("Aluno não encontrado para exclusão. ID: {AlunoId}", id);
                    return NotFound(new { message = "Aluno não encontrado" });
                }

                _logger.LogInformation("Aluno excluído com sucesso. ID: {AlunoId}", id);
                
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
    }
} 