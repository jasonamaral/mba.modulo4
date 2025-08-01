using Conteudo.Application.DTOs;

namespace Conteudo.Application.Interfaces.Services;

public interface ICursoAppService
{
    Task<IEnumerable<CursoDto>> ObterTodosAsync(bool includeAulas = false);
    Task<CursoDto?> ObterPorIdAsync(Guid id, bool includeAulas = false);
    Task<IEnumerable<CursoDto>> GetByCategoriaIdAsync(Guid categoriaId, bool includeAulas = false);
    Task<IEnumerable<CursoDto>> GetAtivosAsync(bool includeAulas = false);
    Task<IEnumerable<CursoDto>> SearchAsync(string searchTerm, bool includeAulas = false);
} 