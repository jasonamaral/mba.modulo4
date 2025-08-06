using Alunos.Application.DTOs.Review;

namespace Alunos.Application.Interfaces.Services;

public interface IMatriculaAppService
{
    Task<IEnumerable<MatriculaResumoDto>> ListarMatriculasAsync(
        int pagina, int tamanhoPagina, string? status = null, Guid? cursoId = null);

    Task<MatriculaDto?> ObterMatriculaPorIdAsync(Guid id);

    Task<MatriculaDto> CriarMatriculaAsync(Guid alunoId, MatriculaCadastroDto dto);

    Task<MatriculaDto?> AtualizarMatriculaAsync(Guid id, MatriculaAtualizarDto dto);

    Task<MatriculaDto?> ConcluirMatriculaAsync(Guid id, MatriculaConclusaoDto dto);

    /// <summary>

    Task<MatriculaDto?> CancelarMatriculaAsync(Guid id, MatriculaCancelamentoDto dto);

    Task<IEnumerable<ProgressoDto>> ObterProgressoMatriculaAsync(Guid id);

    Task<bool> VerificarExistenciaMatriculaAsync(Guid id);
}