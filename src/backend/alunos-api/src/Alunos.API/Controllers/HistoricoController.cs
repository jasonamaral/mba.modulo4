using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Alunos.Application.DTOs;
using Alunos.Application.Interfaces.Services;

namespace Alunos.API.Controllers
{
    /// <summary>
    /// Controller para operações de Histórico
    /// </summary>
    [ApiController]
    [Route("api/historico")]
    [Authorize]
    public class HistoricoController : ControllerBase
    {
        private readonly IHistoricoAlunoAppService _historicoAppService;
        private readonly ILogger<HistoricoController> _logger;

        /// <summary>
        /// Construtor do controller
        /// </summary>
        /// <param name="historicoAppService">Serviço de aplicação de histórico</param>
        /// <param name="logger">Logger</param>
        public HistoricoController(
            IHistoricoAlunoAppService historicoAppService,
            ILogger<HistoricoController> logger)
        {
            _historicoAppService = historicoAppService;
            _logger = logger;
        }

        /// <summary>
        /// Listar histórico com filtros
        /// </summary>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="filtros">Filtros de pesquisa</param>
        /// <returns>Lista paginada de histórico</returns>
        [HttpGet]
        [ProducesResponseType(typeof(HistoricoAlunoPaginadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarHistorico(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10,
            [FromQuery] HistoricoAlunoFiltroDto? filtros = null)
        {
            try
            {
                var historico = await _historicoAppService.ListarHistoricoAsync(
                    pagina, tamanhoPagina, filtros);

                _logger.LogInformation("Listagem de histórico realizada. Página: {Pagina}, Tamanho: {Tamanho}", 
                    pagina, tamanhoPagina);

                return Ok(historico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao listar histórico");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter histórico por ID
        /// </summary>
        /// <param name="id">ID do histórico</param>
        /// <returns>Dados do histórico</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(HistoricoAlunoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterHistoricoPorId(Guid id)
        {
            try
            {
                var historico = await _historicoAppService.ObterHistoricoPorIdAsync(id);

                if (historico == null)
                {
                    _logger.LogWarning("Histórico não encontrado. ID: {HistoricoId}", id);
                    return NotFound(new { message = "Histórico não encontrado" });
                }

                _logger.LogInformation("Histórico obtido com sucesso. ID: {HistoricoId}", id);

                return Ok(historico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter histórico {HistoricoId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter histórico por aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="tipoAcao">Filtro por tipo de ação</param>
        /// <returns>Lista paginada de histórico do aluno</returns>
        [HttpGet("aluno/{alunoId:guid}")]
        [ProducesResponseType(typeof(HistoricoAlunoPaginadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterHistoricoPorAluno(
            Guid alunoId,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10,
            [FromQuery] string? tipoAcao = null)
        {
            try
            {
                var historico = await _historicoAppService.ObterHistoricoPorAlunoAsync(
                    alunoId, pagina, tamanhoPagina, tipoAcao);

                _logger.LogInformation("Histórico do aluno obtido com sucesso. Aluno: {AlunoId}", alunoId);

                return Ok(historico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter histórico do aluno {AlunoId}", alunoId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Criar registro de histórico
        /// </summary>
        /// <param name="dto">Dados do histórico</param>
        /// <returns>Histórico criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(HistoricoAlunoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarHistorico([FromBody] HistoricoAlunoCadastroDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var historico = await _historicoAppService.CriarHistoricoAsync(dto);
                
                _logger.LogInformation("Histórico criado com sucesso. ID: {HistoricoId}, Aluno: {AlunoId}", 
                    historico.Id, dto.AlunoId);
                
                return CreatedAtAction(
                    nameof(ObterHistoricoPorId),
                    new { id = historico.Id },
                    historico);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao criar histórico: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao criar histórico");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter atividades recentes do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="limite">Limite de registros</param>
        /// <returns>Lista de atividades recentes</returns>
        [HttpGet("aluno/{alunoId:guid}/recentes")]
        [ProducesResponseType(typeof(IEnumerable<HistoricoAlunoResumoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterAtividadesRecentes(
            Guid alunoId,
            [FromQuery] int limite = 10)
        {
            try
            {
                var atividades = await _historicoAppService.ObterAtividadesRecentesAsync(alunoId, limite);

                _logger.LogInformation("Atividades recentes do aluno obtidas com sucesso. Aluno: {AlunoId}", alunoId);

                return Ok(atividades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter atividades recentes do aluno {AlunoId}", alunoId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter estatísticas de atividade do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="dataInicial">Data inicial para análise</param>
        /// <param name="dataFinal">Data final para análise</param>
        /// <returns>Estatísticas de atividade</returns>
        [HttpGet("aluno/{alunoId:guid}/estatisticas")]
        [ProducesResponseType(typeof(HistoricoAlunoEstatisticasDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterEstatisticasAtividade(
            Guid alunoId,
            [FromQuery] DateTime? dataInicial = null,
            [FromQuery] DateTime? dataFinal = null)
        {
            try
            {
                var estatisticas = await _historicoAppService.ObterEstatisticasAtividadeAsync(
                    alunoId, dataInicial, dataFinal);

                if (estatisticas == null)
                {
                    _logger.LogWarning("Estatísticas de atividade não encontradas. Aluno: {AlunoId}", alunoId);
                    return NotFound(new { message = "Aluno não encontrado" });
                }

                _logger.LogInformation("Estatísticas de atividade obtidas com sucesso. Aluno: {AlunoId}", alunoId);

                return Ok(estatisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter estatísticas de atividade do aluno {AlunoId}", alunoId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter histórico por tipo de ação
        /// </summary>
        /// <param name="tipoAcao">Tipo de ação</param>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="dataInicial">Data inicial para filtro</param>
        /// <param name="dataFinal">Data final para filtro</param>
        /// <returns>Lista paginada de histórico por tipo</returns>
        [HttpGet("tipo/{tipoAcao}")]
        [ProducesResponseType(typeof(HistoricoAlunoPaginadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterHistoricoPorTipo(
            string tipoAcao,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10,
            [FromQuery] DateTime? dataInicial = null,
            [FromQuery] DateTime? dataFinal = null)
        {
            try
            {
                var historico = await _historicoAppService.ObterHistoricoPorTipoAsync(
                    tipoAcao, pagina, tamanhoPagina, dataInicial, dataFinal);

                _logger.LogInformation("Histórico por tipo obtido com sucesso. Tipo: {TipoAcao}", tipoAcao);

                return Ok(historico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter histórico por tipo {TipoAcao}", tipoAcao);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Registrar login do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <param name="userAgent">User Agent</param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("aluno/{alunoId:guid}/login")]
        [ProducesResponseType(typeof(HistoricoAlunoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarLogin(
            Guid alunoId,
            [FromQuery] string enderecoIP = "",
            [FromQuery] string userAgent = "")
        {
            try
            {
                var historico = await _historicoAppService.RegistrarLoginAsync(alunoId, enderecoIP, userAgent);
                
                _logger.LogInformation("Login registrado com sucesso. Aluno: {AlunoId}", alunoId);
                
                return CreatedAtAction(
                    nameof(ObterHistoricoPorId),
                    new { id = historico.Id },
                    historico);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao registrar login do aluno {AlunoId}: {Message}", 
                    alunoId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao registrar login do aluno {AlunoId}", alunoId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Registrar logout do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <param name="userAgent">User Agent</param>
        /// <returns>Resultado da operação</returns>
        [HttpPost("aluno/{alunoId:guid}/logout")]
        [ProducesResponseType(typeof(HistoricoAlunoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarLogout(
            Guid alunoId,
            [FromQuery] string enderecoIP = "",
            [FromQuery] string userAgent = "")
        {
            try
            {
                var historico = await _historicoAppService.RegistrarLogoutAsync(alunoId, enderecoIP, userAgent);
                
                _logger.LogInformation("Logout registrado com sucesso. Aluno: {AlunoId}", alunoId);
                
                return CreatedAtAction(
                    nameof(ObterHistoricoPorId),
                    new { id = historico.Id },
                    historico);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao registrar logout do aluno {AlunoId}: {Message}", 
                    alunoId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao registrar logout do aluno {AlunoId}", alunoId);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }
    }
} 