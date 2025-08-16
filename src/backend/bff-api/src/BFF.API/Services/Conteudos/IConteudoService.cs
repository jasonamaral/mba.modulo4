using BFF.API.Models.Request;
using BFF.Domain.DTOs;
using Core.Communication;
using Core.Communication.Filters;

namespace BFF.API.Services.Conteudos;

public interface IConteudoService
{
    Task<ResponseResult<CursoDto>> ObterCursoPorIdAsync(Guid cursoId, bool includeAulas = false);
    Task<ResponseResult<PagedResult<CursoDto>>> ObterTodosCursosAsync(CursoFilter filter);
    Task<ResponseResult<IEnumerable<CursoDto>>> ObterPorCategoriaIdAsync(Guid categoriaId, bool includeAulas = false);
    Task<ResponseResult<IEnumerable<CategoriaDto>>> ObterTodasCategoriasAsync();
    Task<ResponseResult<ConteudoProgramaticoDto>> ObterConteudoProgramaticoPorCursoIdAsync(Guid categoriaId, bool includeAulas = false);
    Task<ResponseResult<Guid?>> AdicionarCursoAsync(CursoCriarRequest curso);
    Task<ResponseResult<CursoDto>> AtualizarCursoAsync(Guid id, AtualizarCursoRequest curso);
    Task<ResponseResult<bool>> ExcluirCursoAsync(Guid cursoId);
    Task<ResponseResult<Guid?>> AdicionarAulaAsync(Guid cursoId, AulaDto aula);
    Task<ResponseResult<AulaDto>> AtualizarAulaAsync(Guid cursoId, AulaDto aula);
    Task<ResponseResult<bool>> ExcluirAulaAsync(Guid cursoId, Guid aulaId);
}
