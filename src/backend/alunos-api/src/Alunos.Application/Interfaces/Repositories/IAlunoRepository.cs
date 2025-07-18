using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alunos.Domain.Entities;

namespace Alunos.Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface do repositório de Aluno
    /// </summary>
    public interface IAlunoRepository
    {
        /// <summary>
        /// Obtém um aluno por ID
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Aluno encontrado</returns>
        Task<Aluno?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtém um aluno por código de usuário de autenticação
        /// </summary>
        /// <param name="codigoUsuario">Código do usuário</param>
        /// <returns>Aluno encontrado</returns>
        Task<Aluno?> GetByCodigoUsuarioAsync(Guid codigoUsuario);

        /// <summary>
        /// Obtém um aluno por email
        /// </summary>
        /// <param name="email">Email do aluno</param>
        /// <returns>Aluno encontrado</returns>
        Task<Aluno?> GetByEmailAsync(string email);

        /// <summary>
        /// Obtém todos os alunos
        /// </summary>
        /// <param name="includeMatriculas">Se deve incluir matrículas</param>
        /// <returns>Lista de alunos</returns>
        Task<IEnumerable<Aluno>> GetAllAsync(bool includeMatriculas = false);

        /// <summary>
        /// Obtém alunos ativos
        /// </summary>
        /// <param name="includeMatriculas">Se deve incluir matrículas</param>
        /// <returns>Lista de alunos ativos</returns>
        Task<IEnumerable<Aluno>> GetAlunosAtivosAsync(bool includeMatriculas = false);

        /// <summary>
        /// Busca alunos por nome
        /// </summary>
        /// <param name="nome">Nome para busca</param>
        /// <param name="includeMatriculas">Se deve incluir matrículas</param>
        /// <returns>Lista de alunos encontrados</returns>
        Task<IEnumerable<Aluno>> BuscarPorNomeAsync(string nome, bool includeMatriculas = false);

        /// <summary>
        /// Obtém alunos por cidade
        /// </summary>
        /// <param name="cidade">Cidade</param>
        /// <param name="includeMatriculas">Se deve incluir matrículas</param>
        /// <returns>Lista de alunos da cidade</returns>
        Task<IEnumerable<Aluno>> GetByCidadeAsync(string cidade, bool includeMatriculas = false);

        /// <summary>
        /// Obtém alunos por estado
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="includeMatriculas">Se deve incluir matrículas</param>
        /// <returns>Lista de alunos do estado</returns>
        Task<IEnumerable<Aluno>> GetByEstadoAsync(string estado, bool includeMatriculas = false);

        /// <summary>
        /// Obtém alunos matriculados em um curso
        /// </summary>
        /// <param name="cursoId">ID do curso</param>
        /// <returns>Lista de alunos matriculados</returns>
        Task<IEnumerable<Aluno>> GetAlunosMatriculadosNoCursoAsync(Guid cursoId);

        /// <summary>
        /// Obtém contagem de alunos
        /// </summary>
        /// <returns>Quantidade total de alunos</returns>
        Task<int> GetCountAsync();

        /// <summary>
        /// Obtém contagem de alunos ativos
        /// </summary>
        /// <returns>Quantidade de alunos ativos</returns>
        Task<int> GetCountAtivosAsync();

        /// <summary>
        /// Verifica se existe aluno com o email
        /// </summary>
        /// <param name="email">Email para verificar</param>
        /// <param name="excluirId">ID para excluir da verificação</param>
        /// <returns>True se existe</returns>
        Task<bool> ExisteEmailAsync(string email, Guid? excluirId = null);

        /// <summary>
        /// Adiciona um novo aluno
        /// </summary>
        /// <param name="aluno">Aluno a ser adicionado</param>
        /// <returns>Aluno adicionado</returns>
        Task<Aluno> AddAsync(Aluno aluno);

        /// <summary>
        /// Atualiza um aluno
        /// </summary>
        /// <param name="aluno">Aluno a ser atualizado</param>
        /// <returns>Aluno atualizado</returns>
        Task<Aluno> UpdateAsync(Aluno aluno);

        /// <summary>
        /// Remove um aluno
        /// </summary>
        /// <param name="aluno">Aluno a ser removido</param>
        /// <returns>Task</returns>
        Task DeleteAsync(Aluno aluno);

        /// <summary>
        /// Remove um aluno por ID
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Task</returns>
        Task DeleteByIdAsync(Guid id);

        /// <summary>
        /// Salva as alterações
        /// </summary>
        /// <returns>Número de registros afetados</returns>
        Task<int> SaveChangesAsync();
    }
} 