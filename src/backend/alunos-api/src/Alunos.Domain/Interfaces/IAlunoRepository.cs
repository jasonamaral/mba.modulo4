using Alunos.Domain.Entities;
using Alunos.Domain.ValueObjects;
using Core.Data;

namespace Alunos.Domain.Interfaces;

public interface IAlunoRepository : IRepository<Aluno>
{
    #region Aluno

    Task<Aluno> ObterPorIdAsync(Guid alunoId);

    Task<Aluno> ObterPorEmailAsync(string email);

    Task<Aluno> ObterPorCodigoUsuarioAsync(Guid codigoUsuario);

    Task<bool> ExisteEmailAsync(string email);

    Task AdicionarAsync(Aluno aluno);

    Task AtualizarAsync(Aluno aluno);

    #endregion Aluno

    #region Matrícula

    Task AdicionarMatriculaCursoAsync(MatriculaCurso matriculaCurso);

    Task AdicionarCertificadoMatriculaCursoAsync(Certificado certificado);

    Task<MatriculaCurso> ObterMatriculaPorIdAsync(Guid matriculaId);

    Task<MatriculaCurso> ObterMatriculaPorAlunoECursoAsync(Guid alunoId, Guid cursoId);

    #endregion Matrícula

    #region Certificado

    Task AtualizarEstadoHistoricoAprendizadoAsync(HistoricoAprendizado historicoAntigo, HistoricoAprendizado historicoNovo);

    Task<Certificado> ObterCertificadoPorMatriculaAsync(Guid matriculaId);

    #endregion Certificado
}
