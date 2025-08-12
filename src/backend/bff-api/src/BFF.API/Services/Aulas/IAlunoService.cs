using BFF.Domain.DTOs.Alunos.Request;
using BFF.Domain.DTOs.Alunos.Response;
using Core.Communication;

namespace BFF.API.Services.Aulas;
public interface IAlunoService
{
    Task<ResponseResult<AlunoDto>> ObterAlunoPorIdAsync(Guid alunoId);
    Task<ResponseResult<EvolucaoAlunoDto>> ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid alunoId);
    Task<ResponseResult<ICollection<MatriculaCursoDto>>> ObterMatriculasPorAlunoIdAsync(Guid alunoId);
    Task<ResponseResult<CertificadoDto>> ObterCertificadoPorMatriculaIdAsync(Guid matriculaId);
    Task<ResponseResult<ICollection<AulaCursoDto>>> ObterAulasPorMatriculaIdAsync(Guid matriculaId);

    Task<ResponseResult<Guid>> MatricularAlunoAsync(MatriculaCursoRequest dto);
    Task<ResponseResult<bool>> RegistrarHistoricoAprendizadoAsync(RegistroHistoricoAprendizadoRequest dto);
    Task<ResponseResult<bool>> ConcluirCursoAsync(ConcluirCursoRequest dto);
    Task<ResponseResult<Guid>> SolicitarCertificadoAsync(SolicitaCertificadoRequest dto);
}
