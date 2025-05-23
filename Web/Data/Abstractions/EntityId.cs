namespace Web.Data.Abstractions;

public readonly record struct EntityId<TEntity>(int Value)
{
    public static EntityId<TEntity> Empty => new(0);
    public bool IsEmpty => Value == 0;

    public EntityId<TEntity> Set(int value) =>
        this.IsEmpty ? new EntityId<TEntity>(value)
        : throw new InvalidOperationException("Cannot set a non-empty Id.");
}