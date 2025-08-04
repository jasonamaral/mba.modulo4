using Conteudo.Application.DTOs;
using Core.Communication;
using Core.Communication.Filters;

namespace Conteudo.Application.Interfaces.Services;

public interface ICursoAppService
{
    Task<PagedResult<CursoDto>> ObterTodosAsync(CursoFilter filter);
    Task<IEnumerable<CursoDto>> ObterTodosAsync(bool includeAulas = false);
    Task<CursoDto?> ObterPorIdAsync(Guid id, bool includeAulas = false);
    Task<IEnumerable<CursoDto>> ObterPorCategoriaIdAsync(Guid categoriaId, bool includeAulas = false);
} 