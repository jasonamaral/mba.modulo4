using BFF.API.Models.Request;
using BFF.Domain.DTOs;
using Core.Communication;
using Core.Communication.Filters;

namespace BFF.API.Services;

public interface IConteudoService
{
    Task<ResponseResult<CursoDto>> ObterCursoPorId(Guid cursoId, string token, bool includeAulas = false);
    Task<ResponseResult<PagedResult<CursoDto>>> ObterTodosCursos(string token, CursoFilter filter);
    Task<ResponseResult<IEnumerable<CursoDto>>> ObterPorCategoriaIdAsync(string token, Guid categoriaId, bool includeAulas = false);    
    Task<ResponseResult<Guid>> AdicionarCurso(CursoCriarRequest curso, string token);
    Task<ResponseResult<CursoDto>> AtualizarCurso(Guid id, AtualizarCursoRequest curso, string token);
    Task<ResponseResult<bool>> ExcluirCurso(Guid cursoId);
    Task<ResponseResult<Guid>> AdicionarAula(Guid cursoId, AulaDto aula);
    Task<ResponseResult<AulaDto>> AtualizarAula(Guid cursoId, AulaDto aula);
    Task<ResponseResult<bool>> ExcluirAula(Guid cursoId, Guid aulaId);
}
