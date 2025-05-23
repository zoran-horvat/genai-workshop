namespace Web.Data.Abstractions;

public interface IRepository<TEntity, TId>
{
    Task<TEntity?> TryFindAsync(TId id, CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
}