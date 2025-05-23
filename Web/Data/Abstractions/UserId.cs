namespace Web.Data.Abstractions;

public readonly record struct UserId(string Value)
{
    public bool IsEmpty => string.IsNullOrEmpty(Value);

    public UserId EnsureNonEmpty() =>
        !IsEmpty ? this : throw new ArgumentException("UserId cannot be empty.", nameof(Value));

    public static UserId Empty => new UserId(string.Empty);
}