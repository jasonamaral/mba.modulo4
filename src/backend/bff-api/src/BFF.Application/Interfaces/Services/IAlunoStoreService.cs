using BFF.Domain.DTOs;

namespace BFF.Application.Interfaces.Services;

public interface IAlunoStoreService
{
    Task<IReadOnlyList<MatriculaDto>> ListarMatriculasAsync(Guid alunoId);
    Task<MatriculaDto?> ObterMatriculaAsync(Guid alunoId, Guid matriculaId);
    Task<MatriculaDto> CriarMatriculaAsync(Guid alunoId, Guid cursoId, string cursoNome);
    Task<bool> AtualizarMatriculaAsync(Guid alunoId, MatriculaDto matricula);

    Task<ProgressoGeralDto> ObterProgressoAsync(Guid alunoId);
    Task<bool> AtualizarProgressoAulaAsync(Guid alunoId, Guid cursoId, Guid aulaId, decimal percentualAcumulado);

    Task<IReadOnlyList<CertificadoDto>> ListarCertificadosAsync(Guid alunoId);
    Task<CertificadoDto> EmitirCertificadoAsync(Guid alunoId, Guid cursoId, string cursoNome);
}


