using Alunos.Application.DTOs;

namespace Alunos.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de aplicação de Histórico do Aluno
    /// </summary>
    public interface IHistoricoAlunoAppService
    {
        /// <summary>
        /// Listar histórico com filtros
        /// </summary>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="filtros">Filtros de pesquisa</param>
        /// <returns>Lista paginada de histórico</returns>
        Task<HistoricoAlunoPaginadoDto> ListarHistoricoAsync(
            int pagina, int tamanhoPagina, HistoricoAlunoFiltroDto? filtros = null);

        /// <summary>
        /// Obter histórico por ID
        /// </summary>
        /// <param name="id">ID do histórico</param>
        /// <returns>Dados do histórico</returns>
        Task<HistoricoAlunoDto?> ObterHistoricoPorIdAsync(Guid id);

        /// <summary>
        /// Obter histórico por aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="tipoAcao">Filtro por tipo de ação</param>
        /// <returns>Lista paginada de histórico do aluno</returns>
        Task<HistoricoAlunoPaginadoDto> ObterHistoricoPorAlunoAsync(
            Guid alunoId, int pagina, int tamanhoPagina, string? tipoAcao = null);

        /// <summary>
        /// Criar registro de histórico
        /// </summary>
        /// <param name="dto">Dados do histórico</param>
        /// <returns>Histórico criado</returns>
        Task<HistoricoAlunoDto> CriarHistoricoAsync(HistoricoAlunoCadastroDto dto);

        /// <summary>
        /// Obter atividades recentes do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="limite">Limite de registros</param>
        /// <returns>Lista de atividades recentes</returns>
        Task<IEnumerable<HistoricoAlunoResumoDto>> ObterAtividadesRecentesAsync(Guid alunoId, int limite);

        /// <summary>
        /// Obter estatísticas de atividade do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="dataInicial">Data inicial para análise</param>
        /// <param name="dataFinal">Data final para análise</param>
        /// <returns>Estatísticas de atividade</returns>
        Task<HistoricoAlunoEstatisticasDto?> ObterEstatisticasAtividadeAsync(
            Guid alunoId, DateTime? dataInicial = null, DateTime? dataFinal = null);

        /// <summary>
        /// Obter histórico por tipo de ação
        /// </summary>
        /// <param name="tipoAcao">Tipo de ação</param>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="dataInicial">Data inicial para filtro</param>
        /// <param name="dataFinal">Data final para filtro</param>
        /// <returns>Lista paginada de histórico por tipo</returns>
        Task<HistoricoAlunoPaginadoDto> ObterHistoricoPorTipoAsync(
            string tipoAcao, int pagina, int tamanhoPagina, DateTime? dataInicial = null, DateTime? dataFinal = null);

        /// <summary>
        /// Registrar login do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <param name="userAgent">User Agent</param>
        /// <returns>Histórico de login</returns>
        Task<HistoricoAlunoDto> RegistrarLoginAsync(Guid alunoId, string enderecoIP, string userAgent);

        /// <summary>
        /// Registrar logout do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="enderecoIP">Endereço IP</param>
        /// <param name="userAgent">User Agent</param>
        /// <returns>Histórico de logout</returns>
        Task<HistoricoAlunoDto> RegistrarLogoutAsync(Guid alunoId, string enderecoIP, string userAgent);
    }
} 