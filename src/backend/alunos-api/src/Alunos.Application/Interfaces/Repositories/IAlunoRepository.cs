using Alunos.Domain.Entities;

namespace Alunos.Application.Interfaces.Repositories
{
    public interface IAlunoRepository
    {
        Task<Aluno?> GetByIdAsync(Guid id);

        Task<Aluno?> GetByCodigoUsuarioAsync(Guid codigoUsuario);

        Task<Aluno?> GetByEmailAsync(string email);

        Task<IEnumerable<Aluno>> GetAllAsync(bool includeMatriculas = false);

        Task<IEnumerable<Aluno>> GetAlunosAtivosAsync(bool includeMatriculas = false);

        Task<IEnumerable<Aluno>> BuscarPorNomeAsync(string nome, bool includeMatriculas = false);

        Task<IEnumerable<Aluno>> GetByCidadeAsync(string cidade, bool includeMatriculas = false);

        Task<IEnumerable<Aluno>> GetByEstadoAsync(string estado, bool includeMatriculas = false);

        Task<IEnumerable<Aluno>> GetAlunosMatriculadosNoCursoAsync(Guid cursoId);

        Task<int> GetCountAsync();

        Task<int> GetCountAtivosAsync();

        Task<bool> ExisteEmailAsync(string email, Guid? excluirId = null);

        Task<Aluno> AddAsync(Aluno aluno);

        Task<Aluno> UpdateAsync(Aluno aluno);

        Task DeleteAsync(Aluno aluno);

        Task DeleteByIdAsync(Guid id);

        Task<int> SaveChangesAsync();
    }
}