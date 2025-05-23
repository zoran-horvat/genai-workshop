using Web.Models;

namespace Web.Data.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity, TId> GetRepository<TEntity, TId>() where TEntity : class;
    Task CommitAsync(CancellationToken cancellationToken = default);

    IRepository<Company, EntityId<Company>> Companies => GetRepository<Company, EntityId<Company>>();
}