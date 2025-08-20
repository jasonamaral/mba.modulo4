using Core.SharedDtos.Conteudo;

namespace Conteudo.Application.Interfaces.Services;

public interface IAulaAppService
{
    Task<IEnumerable<AulaDto>> ObterTodosAsync(bool includeMateriais = false);

    Task<AulaDto?> ObterPorIdAsync(Guid id, bool includeMateriais = false);

    Task<IEnumerable<AulaDto>> ObterPorCursoIdAsync(Guid cursoId, bool includeMateriais = false);

    Task<IEnumerable<AulaDto>> ObterPublicadasAsync(bool includeMateriais = false);

    Task<IEnumerable<AulaDto>> ObterPublicadasPorCursoIdAsync(Guid cursoId, bool includeMateriais = false);
}
