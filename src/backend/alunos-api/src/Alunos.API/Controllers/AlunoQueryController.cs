using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Alunos.Application.DTOs;
using Alunos.Application.Interfaces.Services;

namespace Alunos.API.Controllers
{
    /// <summary>
    /// Controller para operações de consulta (leitura) de Aluno
    /// </summary>
    [ApiController]
    [Route("api/alunos")]
    [Authorize]
    public class AlunoQueryController : ControllerBase
    {
        private readonly IAlunoAppService _alunoAppService;
        private readonly ILogger<AlunoQueryController> _logger;

        /// <summary>
        /// Construtor do controller
        /// </summary>
        /// <param name="alunoAppService">Serviço de aplicação de aluno</param>
        /// <param name="logger">Logger</param>
        public AlunoQueryController(
            IAlunoAppService alunoAppService,
            ILogger<AlunoQueryController> logger)
        {
            _alunoAppService = alunoAppService;
            _logger = logger;
        }

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

                _logger.LogInformation("Listagem de alunos realizada. Página: {Pagina}, Tamanho: {Tamanho}", 
                    pagina, tamanhoPagina);

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
                    _logger.LogWarning("Aluno não encontrado. ID: {AlunoId}", id);
                    return NotFound(new { message = "Aluno não encontrado" });
                }

                _logger.LogInformation("Aluno obtido com sucesso. ID: {AlunoId}", id);

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
                    _logger.LogWarning("Aluno não encontrado para o código de usuário: {CodigoUsuario}", codigoUsuario);
                    return NotFound(new { message = "Aluno não encontrado" });
                }

                _logger.LogInformation("Aluno obtido por código de usuário. Código: {CodigoUsuario}", codigoUsuario);

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
                    _logger.LogWarning("Perfil do aluno não encontrado. ID: {AlunoId}", id);
                    return NotFound(new { message = "Aluno não encontrado" });
                }

                _logger.LogInformation("Perfil do aluno obtido com sucesso. ID: {AlunoId}", id);

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
                    _logger.LogWarning("Dashboard do aluno não encontrado. ID: {AlunoId}", id);
                    return NotFound(new { message = "Aluno não encontrado" });
                }

                _logger.LogInformation("Dashboard do aluno obtido com sucesso. ID: {AlunoId}", id);

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter dashboard do aluno {AlunoId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter estatísticas do aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Estatísticas do aluno</returns>
        [HttpGet("{id:guid}/estatisticas")]
        [ProducesResponseType(typeof(AlunoEstatisticasDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterEstatisticasAluno(Guid id)
        {
            try
            {
                var estatisticas = await _alunoAppService.ObterEstatisticasAlunoAsync(id);

                if (estatisticas == null)
                {
                    _logger.LogWarning("Estatísticas do aluno não encontradas. ID: {AlunoId}", id);
                    return NotFound(new { message = "Aluno não encontrado" });
                }

                _logger.LogInformation("Estatísticas do aluno obtidas com sucesso. ID: {AlunoId}", id);

                return Ok(estatisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter estatísticas do aluno {AlunoId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Listar matrículas do aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <param name="status">Filtro por status</param>
        /// <returns>Lista de matrículas do aluno</returns>
        [HttpGet("{id:guid}/matriculas")]
        [ProducesResponseType(typeof(IEnumerable<MatriculaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarMatriculasAluno(Guid id, [FromQuery] string? status = null)
        {
            try
            {
                var matriculas = await _alunoAppService.ListarMatriculasAlunoAsync(id, status);

                _logger.LogInformation("Matrículas do aluno listadas com sucesso. ID: {AlunoId}", id);

                return Ok(matriculas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao listar matrículas do aluno {AlunoId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Listar certificados do aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Lista de certificados do aluno</returns>
        [HttpGet("{id:guid}/certificados")]
        [ProducesResponseType(typeof(IEnumerable<CertificadoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarCertificadosAluno(Guid id)
        {
            try
            {
                var certificados = await _alunoAppService.ListarCertificadosAlunoAsync(id);

                _logger.LogInformation("Certificados do aluno listados com sucesso. ID: {AlunoId}", id);

                return Ok(certificados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao listar certificados do aluno {AlunoId}", id);
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
    }
} 