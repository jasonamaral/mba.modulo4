using Conteudo.Application.DTOs;

namespace Conteudo.Application.Interfaces.Services;

public interface ICursoAppService
{
    Task<IEnumerable<CursoDto>> GetAllAsync(bool includeAulas = false);
    Task<CursoDto?> GetByIdAsync(Guid id, bool includeAulas = false);
    Task<IEnumerable<CursoDto>> GetByCategoriaIdAsync(Guid categoriaId, bool includeAulas = false);
    Task<IEnumerable<CursoDto>> GetAtivosAsync(bool includeAulas = false);
    Task<IEnumerable<CursoDto>> SearchAsync(string searchTerm, bool includeAulas = false);
    Task<Guid> CadastrarCursoAsync(CadastroCursoDto dto);
    Task<CursoDto> AtualizarCursoAsync(AtualizarCursoDto dto);
    Task AtivarCursoAsync(Guid id);
    Task DesativarCursoAsync(Guid id);
    Task ExcluirCursoAsync(Guid id);
    Task<bool> ExisteAsync(Guid id);
    Task<bool> ExistePorNomeAsync(string nome, Guid? excludeId = null);
    Task<int> ContarCursosAsync();
    Task<int> ContarCursosAtivosAsync();
    Task<int> ContarCursosPorCategoriaAsync(Guid categoriaId);
} 