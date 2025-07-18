using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alunos.Application.DTOs;

namespace Alunos.Application.Interfaces.Services
{
    /// <summary>
    /// Interface do Application Service de Aluno
    /// </summary>
    public interface IAlunoAppService
    {
        /// <summary>
        /// Obtém um aluno por ID
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Dados do aluno</returns>
        Task<AlunoDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtém um aluno por ID (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Dados do aluno</returns>
        Task<AlunoDto?> ObterAlunoPorIdAsync(Guid id);

        /// <summary>
        /// Obtém um aluno por código de usuário de autenticação
        /// </summary>
        /// <param name="codigoUsuario">Código do usuário</param>
        /// <returns>Dados do aluno</returns>
        Task<AlunoDto?> GetByCodigoUsuarioAsync(Guid codigoUsuario);

        /// <summary>
        /// Obtém um aluno por código de usuário de autenticação (compatibilidade com controllers)
        /// </summary>
        /// <param name="codigoUsuario">Código do usuário</param>
        /// <returns>Dados do aluno</returns>
        Task<AlunoDto?> ObterAlunoPorCodigoUsuarioAsync(Guid codigoUsuario);

        /// <summary>
        /// Obtém um aluno por email
        /// </summary>
        /// <param name="email">Email do aluno</param>
        /// <returns>Dados do aluno</returns>
        Task<AlunoDto?> GetByEmailAsync(string email);

        /// <summary>
        /// Obtém todos os alunos
        /// </summary>
        /// <param name="includeMatriculas">Se deve incluir matrículas</param>
        /// <returns>Lista de alunos</returns>
        Task<IEnumerable<AlunoResumoDto>> GetAllAsync(bool includeMatriculas = false);

        /// <summary>
        /// Listar alunos com paginação e filtros (compatibilidade com controllers)
        /// </summary>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="filtro">Filtro de busca</param>
        /// <param name="ordenacao">Campo de ordenação</param>
        /// <param name="direcao">Direção da ordenação</param>
        /// <returns>Lista de alunos</returns>
        Task<IEnumerable<AlunoResumoDto>> ListarAlunosAsync(
            int pagina, int tamanhoPagina, string? filtro = null, string? ordenacao = null, string? direcao = null);

        /// <summary>
        /// Obtém alunos ativos
        /// </summary>
        /// <param name="includeMatriculas">Se deve incluir matrículas</param>
        /// <returns>Lista de alunos ativos</returns>
        Task<IEnumerable<AlunoResumoDto>> GetAlunosAtivosAsync(bool includeMatriculas = false);

        /// <summary>
        /// Busca alunos por nome
        /// </summary>
        /// <param name="nome">Nome para busca</param>
        /// <param name="includeMatriculas">Se deve incluir matrículas</param>
        /// <returns>Lista de alunos encontrados</returns>
        Task<IEnumerable<AlunoResumoDto>> BuscarPorNomeAsync(string nome, bool includeMatriculas = false);

        /// <summary>
        /// Obtém perfil completo do aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Perfil do aluno</returns>
        Task<AlunoPerfilDto?> GetPerfilAsync(Guid id);

        /// <summary>
        /// Obtém perfil completo do aluno (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Perfil do aluno</returns>
        Task<AlunoPerfilDto?> ObterPerfilAlunoAsync(Guid id);

        /// <summary>
        /// Obtém dashboard do aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Dashboard do aluno</returns>
        Task<AlunoDashboardDto?> GetDashboardAsync(Guid id);

        /// <summary>
        /// Obtém dashboard do aluno (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Dashboard do aluno</returns>
        Task<AlunoDashboardDto?> ObterDashboardAlunoAsync(Guid id);

        /// <summary>
        /// Obtém estatísticas do aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Estatísticas do aluno</returns>
        Task<AlunoEstatisticasDto?> GetEstatisticasAsync(Guid id);

        /// <summary>
        /// Obtém estatísticas do aluno (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Estatísticas do aluno</returns>
        Task<AlunoEstatisticasDto?> ObterEstatisticasAlunoAsync(Guid id);

        /// <summary>
        /// Cria um novo aluno
        /// </summary>
        /// <param name="dto">Dados do aluno</param>
        /// <returns>Aluno criado</returns>
        Task<AlunoDto> CreateAsync(AlunoCadastroDto dto);

        /// <summary>
        /// Cadastrar novo aluno (compatibilidade com controllers)
        /// </summary>
        /// <param name="dto">Dados do aluno</param>
        /// <returns>Aluno criado</returns>
        Task<AlunoDto> CadastrarAlunoAsync(AlunoCadastroDto dto);

        /// <summary>
        /// Atualiza um aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <param name="dto">Dados para atualização</param>
        /// <returns>Aluno atualizado</returns>
        Task<AlunoDto> UpdateAsync(Guid id, AlunoAtualizarDto dto);

        /// <summary>
        /// Atualizar dados do aluno (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <param name="dto">Dados para atualização</param>
        /// <returns>Aluno atualizado</returns>
        Task<AlunoDto?> AtualizarAlunoAsync(Guid id, AlunoAtualizarDto dto);

        /// <summary>
        /// Ativa um aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Aluno ativado</returns>
        Task<AlunoDto> AtivarAsync(Guid id);

        /// <summary>
        /// Ativar aluno (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>True se ativado com sucesso</returns>
        Task<bool> AtivarAlunoAsync(Guid id);

        /// <summary>
        /// Desativa um aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Aluno desativado</returns>
        Task<AlunoDto> DesativarAsync(Guid id);

        /// <summary>
        /// Desativar aluno (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>True se desativado com sucesso</returns>
        Task<bool> DesativarAlunoAsync(Guid id);

        /// <summary>
        /// Remove um aluno
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Task</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Excluir aluno (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>True se excluído com sucesso</returns>
        Task<bool> ExcluirAlunoAsync(Guid id);

        /// <summary>
        /// Matricula um aluno em um curso
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="dto">Dados da matrícula</param>
        /// <returns>Matrícula criada</returns>
        Task<MatriculaDto> MatricularAsync(Guid alunoId, MatriculaCadastroDto dto);

        /// <summary>
        /// Obtém matrículas do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <returns>Lista de matrículas</returns>
        Task<IEnumerable<MatriculaDto>> GetMatriculasAsync(Guid alunoId);

        /// <summary>
        /// Listar matrículas do aluno (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <param name="status">Filtro por status</param>
        /// <returns>Lista de matrículas</returns>
        Task<IEnumerable<MatriculaDto>> ListarMatriculasAlunoAsync(Guid id, string? status = null);

        /// <summary>
        /// Obtém certificados do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <returns>Lista de certificados</returns>
        Task<IEnumerable<CertificadoDto>> GetCertificadosAsync(Guid alunoId);

        /// <summary>
        /// Listar certificados do aluno (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>Lista de certificados</returns>
        Task<IEnumerable<CertificadoDto>> ListarCertificadosAlunoAsync(Guid id);

        /// <summary>
        /// Obtém histórico do aluno
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="filtro">Filtros para o histórico</param>
        /// <returns>Histórico paginado</returns>
        Task<HistoricoAlunoPaginadoDto> GetHistoricoAsync(Guid alunoId, HistoricoAlunoFiltroDto filtro);

        /// <summary>
        /// Verifica se existe aluno com o email
        /// </summary>
        /// <param name="email">Email para verificar</param>
        /// <param name="excluirId">ID para excluir da verificação</param>
        /// <returns>True se existe</returns>
        Task<bool> ExisteEmailAsync(string email, Guid? excluirId = null);

        /// <summary>
        /// Verificar se aluno existe (compatibilidade com controllers)
        /// </summary>
        /// <param name="id">ID do aluno</param>
        /// <returns>True se existe</returns>
        Task<bool> VerificarExistenciaAlunoAsync(Guid id);

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
    }
} 