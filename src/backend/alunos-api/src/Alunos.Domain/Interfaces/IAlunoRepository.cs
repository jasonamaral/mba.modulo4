using Alunos.Domain.Entities;
using Alunos.Domain.ValueObjects;
using Core.Data;

namespace Alunos.Domain.Interfaces;
public interface IAlunoRepository : IRepository<Aluno>
{
    #region Aluno
    Task<Entities.Aluno> ObterPorIdAsync(Guid alunoId);
    Task<Entities.Aluno> ObterPorEmailAsync(string email);
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

    //Task<Aluno?> GetByIdAsync(Guid id);

    //Task<Aluno?> GetByCodigoUsuarioAsync(Guid codigoUsuario);

    //Task<Aluno?> GetByEmailAsync(string email);

    //Task<IEnumerable<Aluno>> GetAllAsync(bool includeMatriculas = false);

    //Task<IEnumerable<Aluno>> GetAlunosAtivosAsync(bool includeMatriculas = false);

    //Task<IEnumerable<Aluno>> BuscarPorNomeAsync(string nome, bool includeMatriculas = false);

    //Task<IEnumerable<Aluno>> GetByCidadeAsync(string cidade, bool includeMatriculas = false);

    //Task<IEnumerable<Aluno>> GetByEstadoAsync(string estado, bool includeMatriculas = false);

    //Task<IEnumerable<Aluno>> GetAlunosMatriculadosNoCursoAsync(Guid cursoId);

    //Task<int> GetCountAsync();

    //Task<int> GetCountAtivosAsync();

    //Task<bool> ExisteEmailAsync(string email, Guid? excluirId = null);

    //Task<Aluno> AddAsync(Aluno aluno);

    //Task<Aluno> UpdateAsync(Aluno aluno);

    //Task DeleteAsync(Aluno aluno);

    //Task DeleteByIdAsync(Guid id);

    //Task<int> SaveChangesAsync();

}
