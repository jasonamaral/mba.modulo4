using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Alunos.Application.DTOs;
using Alunos.Application.Interfaces.Services;

namespace Alunos.API.Controllers
{
    /// <summary>
    /// Controller para operações de Matrícula
    /// </summary>
    [ApiController]
    [Route("api/matriculas")]
    [Authorize]
    public class MatriculaController : ControllerBase
    {
        private readonly IMatriculaAppService _matriculaAppService;
        private readonly ILogger<MatriculaController> _logger;

        /// <summary>
        /// Construtor do controller
        /// </summary>
        /// <param name="matriculaAppService">Serviço de aplicação de matrícula</param>
        /// <param name="logger">Logger</param>
        public MatriculaController(
            IMatriculaAppService matriculaAppService,
            ILogger<MatriculaController> logger)
        {
            _matriculaAppService = matriculaAppService;
            _logger = logger;
        }

        /// <summary>
        /// Listar todas as matrículas
        /// </summary>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="status">Filtro por status</param>
        /// <param name="cursoId">Filtro por curso</param>
        /// <returns>Lista paginada de matrículas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MatriculaResumoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarMatriculas(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10,
            [FromQuery] string? status = null,
            [FromQuery] Guid? cursoId = null)
        {
            try
            {
                var matriculas = await _matriculaAppService.ListarMatriculasAsync(
                    pagina, tamanhoPagina, status, cursoId);

                _logger.LogInformation("Listagem de matrículas realizada. Página: {Pagina}, Tamanho: {Tamanho}", 
                    pagina, tamanhoPagina);

                return Ok(matriculas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao listar matrículas");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter matrícula por ID
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <returns>Dados da matrícula</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(MatriculaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterMatriculaPorId(Guid id)
        {
            try
            {
                var matricula = await _matriculaAppService.ObterMatriculaPorIdAsync(id);

                if (matricula == null)
                {
                    _logger.LogWarning("Matrícula não encontrada. ID: {MatriculaId}", id);
                    return NotFound(new { message = "Matrícula não encontrada" });
                }

                _logger.LogInformation("Matrícula obtida com sucesso. ID: {MatriculaId}", id);

                return Ok(matricula);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter matrícula {MatriculaId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Criar nova matrícula
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="dto">Dados da matrícula</param>
        /// <returns>Matrícula criada</returns>
        [HttpPost("aluno/{alunoId:guid}")]
        [ProducesResponseType(typeof(MatriculaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarMatricula(Guid alunoId, [FromBody] MatriculaCadastroDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var matricula = await _matriculaAppService.CriarMatriculaAsync(alunoId, dto);
                
                _logger.LogInformation("Matrícula criada com sucesso. ID: {MatriculaId}, Aluno: {AlunoId}", 
                    matricula.Id, alunoId);
                
                return CreatedAtAction(
                    nameof(ObterMatriculaPorId),
                    new { id = matricula.Id },
                    matricula);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao criar matrícula para aluno {AlunoId}: {Message}", 
                    alunoId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao criar matrícula para aluno {AlunoId}", alunoId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Atualizar matrícula
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <param name="dto">Dados para atualização</param>
        /// <returns>Matrícula atualizada</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(MatriculaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizarMatricula(Guid id, [FromBody] MatriculaAtualizarDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var matricula = await _matriculaAppService.AtualizarMatriculaAsync(id, dto);
                
                if (matricula == null)
                {
                    _logger.LogWarning("Matrícula não encontrada para atualização. ID: {MatriculaId}", id);
                    return NotFound(new { message = "Matrícula não encontrada" });
                }

                _logger.LogInformation("Matrícula atualizada com sucesso. ID: {MatriculaId}", id);
                
                return Ok(matricula);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao atualizar matrícula {MatriculaId}: {Message}", 
                    id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao atualizar matrícula {MatriculaId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Concluir matrícula
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <param name="dto">Dados de conclusão</param>
        /// <returns>Resultado da operação</returns>
        [HttpPatch("{id:guid}/concluir")]
        [ProducesResponseType(typeof(MatriculaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConcluirMatricula(Guid id, [FromBody] MatriculaConclusaoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var matricula = await _matriculaAppService.ConcluirMatriculaAsync(id, dto);
                
                if (matricula == null)
                {
                    _logger.LogWarning("Matrícula não encontrada para conclusão. ID: {MatriculaId}", id);
                    return NotFound(new { message = "Matrícula não encontrada" });
                }

                _logger.LogInformation("Matrícula concluída com sucesso. ID: {MatriculaId}", id);
                
                return Ok(matricula);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao concluir matrícula {MatriculaId}: {Message}", 
                    id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao concluir matrícula {MatriculaId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Cancelar matrícula
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <param name="dto">Dados de cancelamento</param>
        /// <returns>Resultado da operação</returns>
        [HttpPatch("{id:guid}/cancelar")]
        [ProducesResponseType(typeof(MatriculaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelarMatricula(Guid id, [FromBody] MatriculaCancelamentoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var matricula = await _matriculaAppService.CancelarMatriculaAsync(id, dto);
                
                if (matricula == null)
                {
                    _logger.LogWarning("Matrícula não encontrada para cancelamento. ID: {MatriculaId}", id);
                    return NotFound(new { message = "Matrícula não encontrada" });
                }

                _logger.LogInformation("Matrícula cancelada com sucesso. ID: {MatriculaId}", id);
                
                return Ok(matricula);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao cancelar matrícula {MatriculaId}: {Message}", 
                    id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao cancelar matrícula {MatriculaId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter progresso da matrícula
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <returns>Progresso da matrícula</returns>
        [HttpGet("{id:guid}/progresso")]
        [ProducesResponseType(typeof(IEnumerable<ProgressoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterProgressoMatricula(Guid id)
        {
            try
            {
                var progresso = await _matriculaAppService.ObterProgressoMatriculaAsync(id);

                _logger.LogInformation("Progresso da matrícula obtido com sucesso. ID: {MatriculaId}", id);

                return Ok(progresso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter progresso da matrícula {MatriculaId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Verificar se matrícula existe
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <returns>Resultado da verificação</returns>
        [HttpHead("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerificarExistenciaMatricula(Guid id)
        {
            try
            {
                var existe = await _matriculaAppService.VerificarExistenciaMatriculaAsync(id);

                if (!existe)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao verificar existência da matrícula {MatriculaId}", id);
                return StatusCode(500);
            }
        }
    }
} 