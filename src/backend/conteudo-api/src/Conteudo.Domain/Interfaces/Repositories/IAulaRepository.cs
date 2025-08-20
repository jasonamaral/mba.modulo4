using Conteudo.Domain.Entities;
using Core.Data;

namespace Conteudo.Domain.Interfaces.Repositories;

public interface IAulaRepository : IRepository<Aula>
{
    Task<IEnumerable<Aula>> ObterTodosAsync(bool includeMateriais = false);

    Task<Aula?> ObterPorIdAsync(Guid id, bool includeMateriais = false);

    Task<IEnumerable<Aula>> ObterPorCursoIdAsync(Guid cursoId, bool includeMateriais = false);

    Task<IEnumerable<Aula>> ObterPublicadasAsync(bool includeMateriais = false);

    Task<IEnumerable<Aula>> ObterPublicadasPorCursoIdAsync(Guid cursoId, bool includeMateriais = false);

    Task<bool> ExisteAsync(Guid id);

    Task<bool> ExistePorNumeroAsync(Guid cursoId, int numero, Guid? excludeId = null);

    Task<Aula> CadastrarAulaAsync(Aula aula);

    Task<Aula> AtualizarAulaAsync(Aula aula);

    Task PublicarAulaAsync(Guid id);

    Task DespublicarAulaAsync(Guid id);

    Task ExcluirAulaAsync(Guid id);

    Task<int> ContarAulasAsync();

    Task<int> ContarAulasPorCursoAsync(Guid cursoId);

    Task<int> ContarAulasPublicadasAsync();
}
