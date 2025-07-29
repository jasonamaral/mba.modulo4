using Core.Data;

namespace Conteudo.Domain.Common
{
    public interface IRepository<T> : IDisposable where T : IRaizAgregacao
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
