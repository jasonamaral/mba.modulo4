using BFF.Domain.DTOs.Alunos.Request;
using BFF.Domain.DTOs.Alunos.Response;
using Core.Communication;

namespace BFF.API.Services.Aulas;
public interface IAulaService
{
    Task<ResponseResult<AlunoDto>> ObterAlunoPorIdAsync(Guid alunoId, string token);
    Task<ResponseResult<EvolucaoAlunoDto>> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId, string token);
    Task<ResponseResult<ICollection<MatriculaCursoDto>>> ObterMatriculasPorAlunoIdAsync(Guid alunoId, string token);
    Task<ResponseResult<CertificadoDto>> ObterCertificadoPorMatriculaIdAsync(Guid matriculaId, string token);
    Task<ResponseResult<ICollection<AulaCursoDto>>> ObterAulasPorMatriculaIdAsync(Guid matriculaId, string token);

    Task<ResponseResult<Guid>> MatricularAlunoAsync(MatriculaCursoRequest dto, string token);
    Task<ResponseResult<bool>> RegistrarHistoricoAprendizadoAsync(RegistroHistoricoAprendizadoRequest dto, string token);
    Task<ResponseResult<bool>> ConcluirCursoAsync(ConcluirCursoRequest dto, string token);
    Task<ResponseResult<Guid>> SolicitarCertificadoAsync(SolicitaCertificadoRequest dto, string token);
}
