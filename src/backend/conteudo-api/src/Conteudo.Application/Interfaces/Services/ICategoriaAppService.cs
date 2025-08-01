using Conteudo.Application.DTOs;

namespace Conteudo.Application.Interfaces.Services;

public interface ICategoriaAppService
{
    Task<IEnumerable<CategoriaDto>> ObterTodasCategoriasAsync();
    Task<CategoriaDto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<CategoriaDto>> GetAtivasAsync(bool includeCursos = false);
    Task<IEnumerable<CategoriaDto>> GetOrderedAsync(bool includeCursos = false);
    Task<Guid> CadastrarCategoriaAsync(CadastroCategoriaDto dto);
    Task<CategoriaDto> AtualizarCategoriaAsync(AtualizarCategoriaDto dto);
    Task AtivarCategoriaAsync(Guid id);
    Task DesativarCategoriaAsync(Guid id);
    Task ExcluirCategoriaAsync(Guid id);
    Task<bool> ExisteAsync(Guid id);
    Task<bool> ExistePorNomeAsync(string nome, Guid? excludeId = null);
    Task<int> ContarCategoriasAsync();
    Task<int> ContarCategoriasAtivasAsync();
} 