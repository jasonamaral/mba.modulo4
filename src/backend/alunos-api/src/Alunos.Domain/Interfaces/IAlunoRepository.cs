using Alunos.Domain.Entities;
using Alunos.Domain.ValueObjects;
using Core.Data;

namespace Alunos.Domain.Interfaces;
public interface IAlunoRepository : IRepository<Aluno>
{
    #region Aluno
    Task<Entities.Aluno> ObterPorIdAsync(Guid alunoId);
    Task<Entities.Aluno> ObterPorEmailAsync(string email);
    Task<Entities.Aluno> ObterPorCodigoUsuarioAsync(Guid codigoUsuario);
    Task<bool> ExisteEmailAsync(string email);
    Task AdicionarAsync(Entities.Aluno aluno);
    Task AtualizarAsync(Entities.Aluno aluno);
    #endregion

    #region Matrícula
    Task AdicionarMatriculaCursoAsync(MatriculaCurso matriculaCurso);
    Task AdicionarCertificadoMatriculaCursoAsync(Certificado certificado);
    Task<MatriculaCurso> ObterMatriculaPorIdAsync(Guid matriculaId);
    Task<MatriculaCurso> ObterMatriculaPorAlunoECursoAsync(Guid alunoId, Guid cursoId);
    #endregion

    #region Certificado
    Task AtualizarEstadoHistoricoAprendizadoAsync(HistoricoAprendizado historicoAntigo, HistoricoAprendizado historicoNovo);
    Task<Certificado> ObterCertificadoPorMatriculaAsync(Guid matriculaId);
    #endregion
}
