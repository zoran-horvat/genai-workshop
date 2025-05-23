namespace Web.Data.Abstractions;

public interface IQuery<T>
{
    Task<IEnumerable<T>> GetAllAsync();
}