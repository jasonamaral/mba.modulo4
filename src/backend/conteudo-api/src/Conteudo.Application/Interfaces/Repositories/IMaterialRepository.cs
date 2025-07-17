using Conteudo.Domain.Entities;

namespace Conteudo.Application.Interfaces.Repositories;

public interface IMaterialRepository
{
    Task<IEnumerable<Material>> GetAllAsync();
    Task<Material?> GetByIdAsync(Guid id);
    Task<IEnumerable<Material>> GetByAulaIdAsync(Guid aulaId);
    Task<IEnumerable<Material>> GetAtivosAsync();
    Task<IEnumerable<Material>> GetAtivosByAulaIdAsync(Guid aulaId);
    Task<IEnumerable<Material>> GetObrigatoriosByAulaIdAsync(Guid aulaId);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByNomeAsync(Guid aulaId, string nome, Guid? excludeId = null);
    Task<Material> AddAsync(Material material);
    Task<Material> UpdateAsync(Material material);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<int> CountByAulaAsync(Guid aulaId);
    Task<int> CountAtivosByAulaAsync(Guid aulaId);
} 