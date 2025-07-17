using Conteudo.Domain.Entities;

namespace Conteudo.Application.Interfaces.Repositories;

public interface ICursoRepository
{
    Task<IEnumerable<Curso>> GetAllAsync(bool includeAulas = false);
    Task<Curso?> GetByIdAsync(Guid id, bool includeAulas = false);
    Task<IEnumerable<Curso>> GetByCategoriaIdAsync(Guid categoriaId, bool includeAulas = false);
    Task<IEnumerable<Curso>> GetAtivosAsync(bool includeAulas = false);
    Task<IEnumerable<Curso>> SearchAsync(string searchTerm, bool includeAulas = false);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByNomeAsync(string nome, Guid? excludeId = null);
    Task<Curso> AddAsync(Curso curso);
    Task<Curso> UpdateAsync(Curso curso);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<int> CountAtivosAsync();
    Task<int> CountByCategoriaAsync(Guid categoriaId);
} 