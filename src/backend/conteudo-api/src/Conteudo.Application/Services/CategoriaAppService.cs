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

        public Task<IEnumerable<CategoriaDto>> GetAtivasAsync(bool includeCursos = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoriaDto>> GetOrderedAsync(bool includeCursos = false)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> CadastrarCategoriaAsync(CadastroCategoriaDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<CategoriaDto> AtualizarCategoriaAsync(AtualizarCategoriaDto dto)
        {
            throw new NotImplementedException();
        }

        public Task AtivarCategoriaAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task DesativarCategoriaAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task ExcluirCategoriaAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistePorNomeAsync(string nome, Guid? excludeId = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> ContarCategoriasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> ContarCategoriasAtivasAsync()
        {
            throw new NotImplementedException();
        }
    }
}
