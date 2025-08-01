using Conteudo.Domain.Common;
using Conteudo.Domain.Entities;

namespace Conteudo.Domain.Interfaces.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IEnumerable<Categoria>> ObterTodosAsync();
    Task<Categoria?> ObterPorIdAsync(Guid id);
    void Adicionar(Categoria categoria);
    Task<bool> ExistePorNome(string nome);
} 