using System.Data.Common;
using Dapper;
using Web.Data.Abstractions;
using Web.Models;

namespace Web.Data;

public class CompaniesRepository : IRepository<Company, EntityId<Company>>
{
    private readonly DbConnection _connection;
    private readonly DbTransaction? _transaction;
    private readonly UserId _userId;

    public CompaniesRepository(UserId userId, DbConnection connection, DbTransaction? transaction = null) =>
        (_connection, _transaction, _userId) = (connection, transaction, userId.EnsureNonEmpty());

    public async Task<Company?> TryFindAsync(EntityId<Company> id, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT c.Id, c.Name, c.TIN, c.Deleted, a.Id, a.StreetAddress, a.City, a.State, a.PostalCode, a.Country
                             FROM business.Companies c
                             INNER JOIN business.Addresses a ON a.CompanyId = c.Id
                             WHERE c.Id = @Id AND c.UserId = @UserId AND c.Deleted = 0";
        var result = await _connection.QueryAsync<CompanyDto, AddressDto, Company>(
            sql,
            (companyDto, addressDto) => companyDto.ToModel(addressDto),
            new { Id = id.Value, UserId = _userId.Value },
            transaction: _transaction,
            splitOn: "Id"
        );
        return result.FirstOrDefault();
    }

    private record CompanyDto(int Id, string Name, string TIN, bool Deleted)
    {
        public Company ToModel(AddressDto address) =>
            new Company(Id: new EntityId<Company>(Id), Name, TIN, address.ToModel());
    }

    private record AddressDto(int Id, string StreetAddress, string City, string State, string PostalCode, string Country)
    {
        public Address ToModel() =>
            new Address(Id: new EntityId<Address>(Id), StreetAddress, City, State, PostalCode, Country);
    }

    public async Task<Company> AddAsync(Company entity, CancellationToken cancellationToken = default)
    {
        if (!entity.Id.IsEmpty) throw new InvalidOperationException("Cannot add an entity that already has an Id.");
        if (!entity.Address.Id.IsEmpty) throw new InvalidOperationException("Cannot add an entity that already has an Id.");

        const string insertCompany = @"INSERT INTO business.Companies (Name, TIN, UserId, Deleted)
                                       OUTPUT INSERTED.Id
                                       VALUES (@Name, @TIN, @UserId, 0);";
        var companyId = await _connection.ExecuteScalarAsync<int>(
            insertCompany,
            new { entity.Name, entity.TIN, UserId = _userId.Value },
            transaction: _transaction
        );

        int addressId = await AddAddress(entity, companyId);

        return entity with
        {
            Id = entity.Id.Set(companyId),
            Address = entity.Address with { Id = entity.Address.Id.Set(addressId) }
        };
    }

    private async Task<int> AddAddress(Company entity, int companyId)
    {
        const string insertAddress = @"INSERT INTO business.Addresses (CompanyId, StreetAddress, City, State, PostalCode, Country)
                                       OUTPUT INSERTED.Id
                                       VALUES (@CompanyId, @StreetAddress, @City, @State, @PostalCode, @Country);";
        var addressId = await _connection.ExecuteScalarAsync<int>(
            insertAddress,
            new
            {
                CompanyId = companyId,
                entity.Address.StreetAddress,
                entity.Address.City,
                entity.Address.State,
                entity.Address.PostalCode,
                entity.Address.Country
            },
            transaction: _transaction
        );
        return addressId;
    }

    public async Task UpdateAsync(Company entity, CancellationToken cancellationToken = default)
    {
        // Update Company
        const string updateCompany = @"UPDATE business.Companies
                                       SET Name = @Name, TIN = @TIN
                                       WHERE Id = @Id AND UserId = @UserId AND Deleted = 0;";
        var companyRows = await _connection.ExecuteAsync(
            updateCompany,
            new { entity.Name, entity.TIN, Id = entity.Id.Value, UserId = _userId.Value },
            transaction: _transaction
        );

        if (companyRows > 0) await UpdateAddress(entity);
    }

    private async Task UpdateAddress(Company entity)
    {
        const string updateAddress = @"UPDATE business.Addresses
                                       SET StreetAddress = @StreetAddress, City = @City,
                                           State = @State, PostalCode = @PostalCode, Country = @Country
                                       WHERE Id = @Id";
        await _connection.ExecuteAsync(
            updateAddress,
            new
            {
                entity.Address.StreetAddress,
                entity.Address.City,
                entity.Address.State,
                entity.Address.PostalCode,
                entity.Address.Country,
                Id = entity.Address.Id.Value
            },
            transaction: _transaction
        );
    }

    public async Task DeleteAsync(EntityId<Company> id, CancellationToken cancellationToken = default)
    {
        // Soft delete: set Deleted = 1
        const string softDeleteCompany = @"UPDATE business.Companies SET Deleted = 1 WHERE Id = @Id AND UserId = @UserId;";
        await _connection.ExecuteAsync(softDeleteCompany, new { Id = id.Value, UserId = _userId.Value }, transaction: _transaction);
    }
}
