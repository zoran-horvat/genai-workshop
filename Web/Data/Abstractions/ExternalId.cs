namespace Web.Data.Abstractions;

public readonly record struct ExternalId<TEntity>(Guid Value)
{
    public static ExternalId<TEntity> CreateNew() => new(Guid.NewGuid());
}