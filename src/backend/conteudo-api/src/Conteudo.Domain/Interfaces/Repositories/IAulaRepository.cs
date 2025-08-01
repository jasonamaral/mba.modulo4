using Conteudo.Domain.Entities;

namespace Conteudo.Domain.Interfaces.Repositories;

public interface IAulaRepository
{
    Task<IEnumerable<Aula>> GetAllAsync(bool includeMateriais = false);
    Task<Aula?> GetByIdAsync(Guid id, bool includeMateriais = false);
    Task<IEnumerable<Aula>> GetByCursoIdAsync(Guid cursoId, bool includeMateriais = false);
    Task<IEnumerable<Aula>> GetPublicadasAsync(bool includeMateriais = false);
    Task<IEnumerable<Aula>> GetPublicadasByCursoIdAsync(Guid cursoId, bool includeMateriais = false);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByNumeroAsync(Guid cursoId, int numero, Guid? excludeId = null);
    Task<Aula> AddAsync(Aula aula);
    Task<Aula> UpdateAsync(Aula aula);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<int> CountByCursoAsync(Guid cursoId);
    Task<int> CountPublicadasAsync();
} 