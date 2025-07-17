using Conteudo.Domain.Entities;

namespace Conteudo.Application.Interfaces.Repositories;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync(bool includeCursos = false);
    Task<Categoria?> GetByIdAsync(Guid id, bool includeCursos = false);
    Task<IEnumerable<Categoria>> GetAtivasAsync(bool includeCursos = false);
    Task<IEnumerable<Categoria>> GetOrderedAsync(bool includeCursos = false);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByNomeAsync(string nome, Guid? excludeId = null);
    Task<Categoria> AddAsync(Categoria categoria);
    Task<Categoria> UpdateAsync(Categoria categoria);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<int> CountAtivasAsync();
} 