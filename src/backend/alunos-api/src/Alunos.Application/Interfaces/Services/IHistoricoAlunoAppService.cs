using Alunos.Application.DTOs;

namespace Alunos.Application.Interfaces.Services;

public interface IHistoricoAlunoAppService
{
    Task<HistoricoAlunoPaginadoDto> ListarHistoricoAsync(
        int pagina, int tamanhoPagina, HistoricoAlunoFiltroDto? filtros = null);

    Task<HistoricoAlunoDto?> ObterHistoricoPorIdAsync(Guid id);

    Task<HistoricoAlunoPaginadoDto> ObterHistoricoPorAlunoAsync(
        Guid alunoId, int pagina, int tamanhoPagina, string? tipoAcao = null);

    Task<HistoricoAlunoDto> CriarHistoricoAsync(HistoricoAlunoCadastroDto dto);

    Task<IEnumerable<HistoricoAlunoResumoDto>> ObterAtividadesRecentesAsync(Guid alunoId, int limite);

    Task<HistoricoAlunoEstatisticasDto?> ObterEstatisticasAtividadeAsync(
        Guid alunoId, DateTime? dataInicial = null, DateTime? dataFinal = null);

    Task<HistoricoAlunoPaginadoDto> ObterHistoricoPorTipoAsync(
        string tipoAcao, int pagina, int tamanhoPagina, DateTime? dataInicial = null, DateTime? dataFinal = null);

    Task<HistoricoAlunoDto> RegistrarLoginAsync(Guid alunoId, string enderecoIP, string userAgent);

    Task<HistoricoAlunoDto> RegistrarLogoutAsync(Guid alunoId, string enderecoIP, string userAgent);
}