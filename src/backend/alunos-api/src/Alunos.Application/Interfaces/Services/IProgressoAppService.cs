using Alunos.Application.DTOs.Review;

namespace Alunos.Application.Interfaces.Services;

public interface IProgressoAppService
{
    Task<ProgressoDto?> ObterProgressoPorIdAsync(Guid id);

    Task<IEnumerable<ProgressoDto>> ObterProgressoPorMatriculaAsync(Guid matriculaId);

    Task<ProgressoDto?> ObterProgressoPorMatriculaEAulaAsync(Guid matriculaId, Guid aulaId);

    Task<ProgressoDto?> AtualizarProgressoAsync(Guid matriculaId, Guid aulaId, ProgressoAtualizarDto dto);

    Task<ProgressoDto?> ConcluirAulaAsync(Guid matriculaId, Guid aulaId, ProgressoConclusaoDto dto);

    Task<ProgressoDto?> IniciarAulaAsync(Guid matriculaId, Guid aulaId);

    Task<ProgressoRelatorioDto?> ObterRelatorioProgressoAsync(Guid matriculaId);

    Task<ProgressoDto?> AbandonarAulaAsync(Guid matriculaId, Guid aulaId);
}