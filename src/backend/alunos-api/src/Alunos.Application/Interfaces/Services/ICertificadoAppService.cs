using Alunos.Application.DTOs.Review;

namespace Alunos.Application.Interfaces.Services;

public interface ICertificadoAppService
{
    Task<IEnumerable<CertificadoDto>> ListarCertificadosAsync(
        int pagina, int tamanhoPagina, string? status = null, Guid? alunoId = null);

    Task<CertificadoDto?> ObterCertificadoPorIdAsync(Guid id);

    Task<CertificadoDto?> ObterCertificadoPorCodigoAsync(string codigo);

    Task<CertificadoDto> GerarCertificadoAsync(CertificadoGerarDto dto);

    Task<CertificadoValidacaoResultadoDto?> ValidarCertificadoAsync(string codigo);

    Task<CertificadoDto?> RenovarCertificadoAsync(Guid id, int validadeDias);

    Task<CertificadoDto?> RevogarCertificadoAsync(Guid id, string motivo);

    Task<CertificadoArquivoDto?> DownloadCertificadoAsync(Guid id);

    Task<bool> VerificarExistenciaCertificadoAsync(Guid id);
}
