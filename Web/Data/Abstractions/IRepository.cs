namespace Web.Data.Abstractions;

public interface IRepository<TEntity>
{
    Task<TEntity?> TryFindAsync(ExternalId<TEntity> id, CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(ExternalId<TEntity> id, CancellationToken cancellationToken = default);
}