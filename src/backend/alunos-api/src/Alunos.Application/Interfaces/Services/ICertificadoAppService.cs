using Alunos.Application.DTOs;

namespace Alunos.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de aplicação de Certificado
    /// </summary>
    public interface ICertificadoAppService
    {
        /// <summary>
        /// Listar certificados com filtros
        /// </summary>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="status">Filtro por status</param>
        /// <param name="alunoId">Filtro por aluno</param>
        /// <returns>Lista paginada de certificados</returns>
        Task<IEnumerable<CertificadoDto>> ListarCertificadosAsync(
            int pagina, int tamanhoPagina, string? status = null, Guid? alunoId = null);

        /// <summary>
        /// Obter certificado por ID
        /// </summary>
        /// <param name="id">ID do certificado</param>
        /// <returns>Dados do certificado</returns>
        Task<CertificadoDto?> ObterCertificadoPorIdAsync(Guid id);

        /// <summary>
        /// Obter certificado por código
        /// </summary>
        /// <param name="codigo">Código do certificado</param>
        /// <returns>Dados do certificado</returns>
        Task<CertificadoDto?> ObterCertificadoPorCodigoAsync(string codigo);

        /// <summary>
        /// Gerar certificado
        /// </summary>
        /// <param name="dto">Dados para geração do certificado</param>
        /// <returns>Certificado gerado</returns>
        Task<CertificadoDto> GerarCertificadoAsync(CertificadoGerarDto dto);

        /// <summary>
        /// Validar certificado
        /// </summary>
        /// <param name="codigo">Código do certificado</param>
        /// <returns>Resultado da validação</returns>
        Task<CertificadoValidacaoResultadoDto?> ValidarCertificadoAsync(string codigo);

        /// <summary>
        /// Renovar certificado
        /// </summary>
        /// <param name="id">ID do certificado</param>
        /// <param name="validadeDias">Dias de validade</param>
        /// <returns>Certificado renovado</returns>
        Task<CertificadoDto?> RenovarCertificadoAsync(Guid id, int validadeDias);

        /// <summary>
        /// Revogar certificado
        /// </summary>
        /// <param name="id">ID do certificado</param>
        /// <param name="motivo">Motivo da revogação</param>
        /// <returns>Certificado revogado</returns>
        Task<CertificadoDto?> RevogarCertificadoAsync(Guid id, string motivo);

        /// <summary>
        /// Download do certificado
        /// </summary>
        /// <param name="id">ID do certificado</param>
        /// <returns>Arquivo do certificado</returns>
        Task<CertificadoArquivoDto?> DownloadCertificadoAsync(Guid id);

        /// <summary>
        /// Verificar se certificado existe
        /// </summary>
        /// <param name="id">ID do certificado</param>
        /// <returns>True se existe, false caso contrário</returns>
        Task<bool> VerificarExistenciaCertificadoAsync(Guid id);
    }

    /// <summary>
    /// DTO para arquivo de certificado
    /// </summary>
    public class CertificadoArquivoDto
    {
        /// <summary>
        /// Conteúdo do arquivo
        /// </summary>
        public byte[] Conteudo { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Tipo de conteúdo
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Nome do arquivo
        /// </summary>
        public string NomeArquivo { get; set; } = string.Empty;
    }
} 