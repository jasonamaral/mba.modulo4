using Alunos.Application.DTOs;

namespace Alunos.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de aplicação de Matrícula
    /// </summary>
    public interface IMatriculaAppService
    {
        /// <summary>
        /// Listar matrículas com filtros
        /// </summary>
        /// <param name="pagina">Número da página</param>
        /// <param name="tamanhoPagina">Tamanho da página</param>
        /// <param name="status">Filtro por status</param>
        /// <param name="cursoId">Filtro por curso</param>
        /// <returns>Lista paginada de matrículas</returns>
        Task<IEnumerable<MatriculaResumoDto>> ListarMatriculasAsync(
            int pagina, int tamanhoPagina, string? status = null, Guid? cursoId = null);

        /// <summary>
        /// Obter matrícula por ID
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <returns>Dados da matrícula</returns>
        Task<MatriculaDto?> ObterMatriculaPorIdAsync(Guid id);

        /// <summary>
        /// Criar nova matrícula
        /// </summary>
        /// <param name="alunoId">ID do aluno</param>
        /// <param name="dto">Dados da matrícula</param>
        /// <returns>Matrícula criada</returns>
        Task<MatriculaDto> CriarMatriculaAsync(Guid alunoId, MatriculaCadastroDto dto);

        /// <summary>
        /// Atualizar matrícula
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <param name="dto">Dados para atualização</param>
        /// <returns>Matrícula atualizada</returns>
        Task<MatriculaDto?> AtualizarMatriculaAsync(Guid id, MatriculaAtualizarDto dto);

        /// <summary>
        /// Concluir matrícula
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <param name="dto">Dados de conclusão</param>
        /// <returns>Matrícula concluída</returns>
        Task<MatriculaDto?> ConcluirMatriculaAsync(Guid id, MatriculaConclusaoDto dto);

        /// <summary>
        /// Cancelar matrícula
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <param name="dto">Dados de cancelamento</param>
        /// <returns>Matrícula cancelada</returns>
        Task<MatriculaDto?> CancelarMatriculaAsync(Guid id, MatriculaCancelamentoDto dto);

        /// <summary>
        /// Obter progresso da matrícula
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <returns>Progresso da matrícula</returns>
        Task<IEnumerable<ProgressoDto>> ObterProgressoMatriculaAsync(Guid id);

        /// <summary>
        /// Verificar se matrícula existe
        /// </summary>
        /// <param name="id">ID da matrícula</param>
        /// <returns>True se existe, false caso contrário</returns>
        Task<bool> VerificarExistenciaMatriculaAsync(Guid id);
    }
} 