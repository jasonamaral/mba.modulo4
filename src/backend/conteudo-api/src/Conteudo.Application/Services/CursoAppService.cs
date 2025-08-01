using AutoMapper;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Conteudo.Domain.Interfaces.Repositories;

namespace Conteudo.Application.Services
{
    public class CursoAppService(ICursoRepository cursoRepository, IMapper mapper) : ICursoAppService
    {
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

        public Task<IEnumerable<CursoDto>> GetByCategoriaIdAsync(Guid categoriaId, bool includeAulas = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CursoDto>> GetAtivosAsync(bool includeAulas = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CursoDto>> SearchAsync(string searchTerm, bool includeAulas = false)
        {
            throw new NotImplementedException();
        }
    }
}
