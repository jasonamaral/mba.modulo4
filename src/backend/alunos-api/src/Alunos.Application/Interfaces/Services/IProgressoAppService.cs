using Alunos.Application.DTOs;

namespace Alunos.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de aplicação de Progresso
    /// </summary>
    public interface IProgressoAppService
    {
        /// <summary>
        /// Obter progresso por ID
        /// </summary>
        /// <param name="id">ID do progresso</param>
        /// <returns>Dados do progresso</returns>
        Task<ProgressoDto?> ObterProgressoPorIdAsync(Guid id);

        /// <summary>
        /// Obter progresso por matrícula
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <returns>Lista de progresso da matrícula</returns>
        Task<IEnumerable<ProgressoDto>> ObterProgressoPorMatriculaAsync(Guid matriculaId);

        /// <summary>
        /// Obter progresso por matrícula e aula
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        /// <returns>Progresso da aula</returns>
        Task<ProgressoDto?> ObterProgressoPorMatriculaEAulaAsync(Guid matriculaId, Guid aulaId);

        /// <summary>
        /// Atualizar progresso
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        /// <param name="dto">Dados de atualização do progresso</param>
        /// <returns>Progresso atualizado</returns>
        Task<ProgressoDto?> AtualizarProgressoAsync(Guid matriculaId, Guid aulaId, ProgressoAtualizarDto dto);

        /// <summary>
        /// Concluir aula
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        /// <param name="dto">Dados de conclusão</param>
        /// <returns>Progresso com aula concluída</returns>
        Task<ProgressoDto?> ConcluirAulaAsync(Guid matriculaId, Guid aulaId, ProgressoConclusaoDto dto);

        /// <summary>
        /// Iniciar aula
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        /// <returns>Progresso com aula iniciada</returns>
        Task<ProgressoDto?> IniciarAulaAsync(Guid matriculaId, Guid aulaId);

        /// <summary>
        /// Obter relatório de progresso
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <returns>Relatório de progresso</returns>
        Task<ProgressoRelatorioDto?> ObterRelatorioProgressoAsync(Guid matriculaId);

        /// <summary>
        /// Abandonar aula
        /// </summary>
        /// <param name="matriculaId">ID da matrícula</param>
        /// <param name="aulaId">ID da aula</param>
        /// <returns>Progresso com aula abandonada</returns>
        Task<ProgressoDto?> AbandonarAulaAsync(Guid matriculaId, Guid aulaId);
    }
} 