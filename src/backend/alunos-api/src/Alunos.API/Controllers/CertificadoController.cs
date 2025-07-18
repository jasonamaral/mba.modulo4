using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Alunos.Application.DTOs;
using Alunos.Application.Interfaces.Services;

namespace Alunos.API.Controllers
{
    /// <summary>
    /// Controller para operações de Certificado
    /// </summary>
    [ApiController]
    [Route("api/certificados")]
    [Authorize]
    public class CertificadoController : ControllerBase
    {
        private readonly ICertificadoAppService _certificadoAppService;
        private readonly ILogger<CertificadoController> _logger;

        /// <summary>
        /// Construtor do controller
        /// </summary>
        /// <param name="certificadoAppService">Serviço de aplicação de certificado</param>
        /// <param name="logger">Logger</param>
        public CertificadoController(
            ICertificadoAppService certificadoAppService,
            ILogger<CertificadoController> logger)
        {
            _certificadoAppService = certificadoAppService;
            _logger = logger;
        }

        /// <summary>
        /// Listar todos os certificados
        /// </summary>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="status">Filtro por status</param>
        /// <param name="alunoId">Filtro por aluno</param>
        /// <returns>Lista paginada de certificados</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CertificadoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarCertificados(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10,
            [FromQuery] string? status = null,
            [FromQuery] Guid? alunoId = null)
        {
            try
            {
                var certificados = await _certificadoAppService.ListarCertificadosAsync(
                    pagina, tamanhoPagina, status, alunoId);

                _logger.LogInformation("Listagem de certificados realizada. Página: {Pagina}, Tamanho: {Tamanho}", 
                    pagina, tamanhoPagina);

                return Ok(certificados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao listar certificados");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter certificado por ID
        /// </summary>
        /// <param name="id">ID do certificado</param>
        /// <returns>Dados do certificado</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CertificadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterCertificadoPorId(Guid id)
        {
            try
            {
                var certificado = await _certificadoAppService.ObterCertificadoPorIdAsync(id);

                if (certificado == null)
                {
                    _logger.LogWarning("Certificado não encontrado. ID: {CertificadoId}", id);
                    return NotFound(new { message = "Certificado não encontrado" });
                }

                _logger.LogInformation("Certificado obtido com sucesso. ID: {CertificadoId}", id);

                return Ok(certificado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter certificado {CertificadoId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter certificado por código
        /// </summary>
        /// <param name="codigo">Código do certificado</param>
        /// <returns>Dados do certificado</returns>
        [HttpGet("codigo/{codigo}")]
        [ProducesResponseType(typeof(CertificadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterCertificadoPorCodigo(string codigo)
        {
            try
            {
                var certificado = await _certificadoAppService.ObterCertificadoPorCodigoAsync(codigo);

                if (certificado == null)
                {
                    _logger.LogWarning("Certificado não encontrado. Código: {Codigo}", codigo);
                    return NotFound(new { message = "Certificado não encontrado" });
                }

                _logger.LogInformation("Certificado obtido por código com sucesso. Código: {Codigo}", codigo);

                return Ok(certificado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao obter certificado por código {Codigo}", codigo);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Gerar certificado
        /// </summary>
        /// <param name="dto">Dados para geração do certificado</param>
        /// <returns>Certificado gerado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CertificadoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GerarCertificado([FromBody] CertificadoGerarDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var certificado = await _certificadoAppService.GerarCertificadoAsync(dto);
                
                _logger.LogInformation("Certificado gerado com sucesso. ID: {CertificadoId}, Matrícula: {MatriculaId}", 
                    certificado.Id, dto.MatriculaId);
                
                return CreatedAtAction(
                    nameof(ObterCertificadoPorId),
                    new { id = certificado.Id },
                    certificado);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao gerar certificado: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao gerar certificado");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Validar certificado
        /// </summary>
        /// <param name="codigo">Código do certificado</param>
        /// <returns>Resultado da validação</returns>
        [HttpGet("validar/{codigo}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CertificadoValidacaoResultadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ValidarCertificado(string codigo)
        {
            try
            {
                var validacao = await _certificadoAppService.ValidarCertificadoAsync(codigo);

                if (validacao == null)
                {
                    _logger.LogWarning("Certificado não encontrado para validação. Código: {Codigo}", codigo);
                    return NotFound(new { message = "Certificado não encontrado" });
                }

                _logger.LogInformation("Certificado validado com sucesso. Código: {Codigo}, Válido: {Valido}", 
                    codigo, validacao.EhValido);

                return Ok(validacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao validar certificado {Codigo}", codigo);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Renovar certificado
        /// </summary>
        /// <param name="id">ID do certificado</param>
        /// <param name="validadeDias">Dias de validade</param>
        /// <returns>Certificado renovado</returns>
        [HttpPatch("{id:guid}/renovar")]
        [ProducesResponseType(typeof(CertificadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RenovarCertificado(Guid id, [FromQuery] int validadeDias = 3650)
        {
            try
            {
                var certificado = await _certificadoAppService.RenovarCertificadoAsync(id, validadeDias);
                
                if (certificado == null)
                {
                    _logger.LogWarning("Certificado não encontrado para renovação. ID: {CertificadoId}", id);
                    return NotFound(new { message = "Certificado não encontrado" });
                }

                _logger.LogInformation("Certificado renovado com sucesso. ID: {CertificadoId}", id);
                
                return Ok(certificado);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao renovar certificado {CertificadoId}: {Message}", 
                    id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao renovar certificado {CertificadoId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Revogar certificado
        /// </summary>
        /// <param name="id">ID do certificado</param>
        /// <param name="motivo">Motivo da revogação</param>
        /// <returns>Resultado da operação</returns>
        [HttpPatch("{id:guid}/revogar")]
        [ProducesResponseType(typeof(CertificadoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RevogarCertificado(Guid id, [FromQuery] string motivo = "")
        {
            try
            {
                var certificado = await _certificadoAppService.RevogarCertificadoAsync(id, motivo);
                
                if (certificado == null)
                {
                    _logger.LogWarning("Certificado não encontrado para revogação. ID: {CertificadoId}", id);
                    return NotFound(new { message = "Certificado não encontrado" });
                }

                _logger.LogInformation("Certificado revogado com sucesso. ID: {CertificadoId}", id);
                
                return Ok(certificado);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação ao revogar certificado {CertificadoId}: {Message}", 
                    id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao revogar certificado {CertificadoId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Download do certificado
        /// </summary>
        /// <param name="id">ID do certificado</param>
        /// <returns>Arquivo do certificado</returns>
        [HttpGet("{id:guid}/download")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DownloadCertificado(Guid id)
        {
            try
            {
                var arquivo = await _certificadoAppService.DownloadCertificadoAsync(id);
                
                if (arquivo == null)
                {
                    _logger.LogWarning("Certificado não encontrado para download. ID: {CertificadoId}", id);
                    return NotFound(new { message = "Certificado não encontrado" });
                }

                _logger.LogInformation("Download do certificado realizado. ID: {CertificadoId}", id);
                
                return File(arquivo.Conteudo, arquivo.ContentType, arquivo.NomeArquivo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao fazer download do certificado {CertificadoId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Verificar se certificado existe
        /// </summary>
        /// <param name="id">ID do certificado</param>
        /// <returns>Resultado da verificação</returns>
        [HttpHead("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerificarExistenciaCertificado(Guid id)
        {
            try
            {
                var existe = await _certificadoAppService.VerificarExistenciaCertificadoAsync(id);

                if (!existe)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao verificar existência do certificado {CertificadoId}", id);
                return StatusCode(500);
            }
        }
    }
} 