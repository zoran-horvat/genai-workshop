using Web.Models;

namespace Web.Data.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    Task CommitAsync(CancellationToken cancellationToken = default);

    IRepository<Company> Companies => GetRepository<Company>();
}