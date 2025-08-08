using BFF.API.Models.Request;
using BFF.Domain.DTOs;
using Core.Communication;
using Core.Communication.Filters;

namespace BFF.API.Services;

public interface IConteudoService
{
    Task<ResponseResult<CursoDto>> ObterCursoPorId(Guid cursoId, bool includeAulas = false);
    Task<ResponseResult<PagedResult<CursoDto>>> ObterTodosCursos(CursoFilter filter);
    Task<ResponseResult<IEnumerable<CursoDto>>> ObterPorCategoriaIdAsync(Guid categoriaId, bool includeAulas = false);    
    Task<ResponseResult<Guid>> AdicionarCurso(CursoCriarRequest curso);
    Task<ResponseResult<CursoDto>> AtualizarCurso(Guid id, AtualizarCursoRequest curso);
    Task<ResponseResult<bool>> ExcluirCurso(Guid cursoId);
    Task<ResponseResult<Guid>> AdicionarAula(Guid cursoId, AulaDto aula);
    Task<ResponseResult<AulaDto>> AtualizarAula(Guid cursoId, AulaDto aula);
    Task<ResponseResult<bool>> ExcluirAula(Guid cursoId, Guid aulaId);
}
