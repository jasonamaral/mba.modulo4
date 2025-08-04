using AutoMapper;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Communication.Filters;

namespace Conteudo.Application.Services
{
    public class CursoAppService(ICursoRepository cursoRepository, IMapper mapper) : ICursoAppService
    {
        public async Task<PagedResult<CursoDto>> ObterTodosAsync(CursoFilter filter)
        {
            var cursos = await cursoRepository.ObterTodosAsync(filter);
            return mapper.Map<PagedResult<CursoDto>>(cursos);
        }
        public async Task<IEnumerable<CursoDto>> ObterTodosAsync(bool includeAulas = false)
        {
            var cursos = await cursoRepository.ObterTodosAsync(includeAulas);
            return mapper.Map<IEnumerable<CursoDto>>(cursos);
        }

        public async Task<CursoDto?> ObterPorIdAsync(Guid id, bool includeAulas = false)
        {
            var curso = await cursoRepository.ObterPorIdAsync(id, includeAulas);
            return mapper.Map<CursoDto>(curso);
        }

        public async Task<IEnumerable<CursoDto>> ObterPorCategoriaIdAsync(Guid categoriaId, bool includeAulas = false)
        {
            var cursos = await cursoRepository.ObterPorCategoriaIdAsync(categoriaId, includeAulas);
            return mapper.Map<IEnumerable<CursoDto>>(cursos);
        }
    }
}
