using Conteudo.Application.DTOs;

namespace Conteudo.Application.Interfaces.Services;

public interface IAulaAppService
{
    Task<IEnumerable<AulaDto>> GetAllAsync(bool includeMateriais = false);
    Task<AulaDto?> GetByIdAsync(Guid id, bool includeMateriais = false);
    Task<IEnumerable<AulaDto>> GetByCursoIdAsync(Guid cursoId, bool includeMateriais = false);
    Task<IEnumerable<AulaDto>> GetPublicadasAsync(bool includeMateriais = false);
    Task<IEnumerable<AulaDto>> GetPublicadasByCursoIdAsync(Guid cursoId, bool includeMateriais = false);
    Task<Guid> CadastrarAulaAsync(CadastroAulaDto dto);
    Task<AulaDto> AtualizarAulaAsync(AtualizarAulaDto dto);
    Task PublicarAulaAsync(Guid id);
    Task DespublicarAulaAsync(Guid id);
    Task ExcluirAulaAsync(Guid id);
    Task<bool> ExisteAsync(Guid id);
    Task<bool> ExistePorNumeroAsync(Guid cursoId, int numero, Guid? excludeId = null);
    Task<int> ContarAulasAsync();
    Task<int> ContarAulasPorCursoAsync(Guid cursoId);
    Task<int> ContarAulasPublicadasAsync();
} 