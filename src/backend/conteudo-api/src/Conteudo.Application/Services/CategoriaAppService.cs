using AutoMapper;
using Conteudo.Application.DTOs;
using Conteudo.Application.Interfaces.Services;
using Conteudo.Domain.Interfaces.Repositories;

namespace Conteudo.Application.Services
{
    public class CategoriaAppService(ICategoriaRepository categoriaRepository
                                    , IMapper mapper) : ICategoriaAppService
    {
        public async Task<IEnumerable<CategoriaDto>> ObterTodasCategoriasAsync()
        {
            var categorias = await categoriaRepository.ObterTodosAsync();
            return mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }

        public async Task<CategoriaDto?> ObterPorIdAsync(Guid id)
        {   
            var categoria = await categoriaRepository.ObterPorIdAsync(id);
            return mapper.Map<CategoriaDto?>(categoria);
        }
    }
}
