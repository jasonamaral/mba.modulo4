namespace Pagamentos.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
