using System.Data.Common;
using Microsoft.Data.SqlClient;
using Web.Data.Abstractions;
using Web.Models;

namespace Web.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbConnection _connection;
    private readonly DbTransaction _transaction;
    private bool _disposed;
    private CompaniesRepository? _companiesRepository;
    private readonly UserId _userId;

    public UnitOfWork(string connectionString, UserId userId)
    {
        _connection = new SqlConnection(connectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();
        _userId = userId.EnsureNonEmpty();
    }

    public IRepository<TEntity, TId> GetRepository<TEntity, TId>() where TEntity : class
    {
        if (typeof(TEntity) == typeof(Company) && typeof(TId) == typeof(EntityId<Company>))
        {
            _companiesRepository ??= new CompaniesRepository(_userId, _connection, _transaction);
            return (IRepository<TEntity, TId>) (object) _companiesRepository;
        }
        throw new NotSupportedException($"Repository for {typeof(TEntity).Name} is not supported.");
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await Task.Yield(); // To keep async signature
        _transaction.Commit();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _transaction.Dispose();
        _connection.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
