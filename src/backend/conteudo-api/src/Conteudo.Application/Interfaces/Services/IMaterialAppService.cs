using Conteudo.Application.DTOs;

namespace Conteudo.Application.Interfaces.Services;

public interface IMaterialAppService
{
    Task<IEnumerable<MaterialDto>> GetAllAsync();
    Task<MaterialDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<MaterialDto>> GetByAulaIdAsync(Guid aulaId);
    Task<IEnumerable<MaterialDto>> GetAtivosAsync();
    Task<IEnumerable<MaterialDto>> GetAtivosByAulaIdAsync(Guid aulaId);
    Task<IEnumerable<MaterialDto>> GetObrigatoriosByAulaIdAsync(Guid aulaId);
    Task<Guid> CadastrarMaterialAsync(CadastroMaterialDto dto);
    Task<MaterialDto> AtualizarMaterialAsync(AtualizarMaterialDto dto);
    Task AtivarMaterialAsync(Guid id);
    Task DesativarMaterialAsync(Guid id);
    Task ExcluirMaterialAsync(Guid id);
    Task<bool> ExisteAsync(Guid id);
    Task<bool> ExistePorNomeAsync(Guid aulaId, string nome, Guid? excludeId = null);
    Task<int> ContarMateriaisAsync();
    Task<int> ContarMateriaisPorAulaAsync(Guid aulaId);
    Task<int> ContarMateriaisAtivosPorAulaAsync(Guid aulaId);
} 