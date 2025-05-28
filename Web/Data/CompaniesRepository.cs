using System.Data.Common;
using Dapper;
using Web.Data.Abstractions;
using Web.Models;

namespace Web.Data;

public class CompaniesRepository : IRepository<Company>
{
    private readonly DbConnection _connection;
    private readonly DbTransaction? _transaction;
    private readonly UserId _userId;

    public CompaniesRepository(UserId userId, DbConnection connection, DbTransaction? transaction = null) =>
        (_connection, _transaction, _userId) = (connection, transaction, userId.EnsureNonEmpty());

    public async Task<Company?> TryFindAsync(ExternalId<Company> externalId, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT c.Id, c.ExternalId, c.Name, c.TIN, c.Deleted, c.CompanyType, a.Id, a.ExternalId, a.StreetAddress, a.City, a.State, a.PostalCode, a.Country, a.AddressKind
                             FROM business.Companies c
                             INNER JOIN business.Addresses a ON a.CompanyId = c.Id
                             WHERE c.ExternalId = @ExternalId AND c.UserId = @UserId AND c.Deleted = 0";
        var result = await _connection.QueryAsync<CompanyDto, AddressDto, Company>(
            sql,
            (companyDto, addressDto) => companyDto.ToModel(addressDto),
            new { ExternalId = externalId.Value, UserId = _userId.Value },
            transaction: _transaction,
            splitOn: "Id"
        );
        return result.FirstOrDefault();
    }

    private record CompanyDto(int Id, Guid ExternalId, string Name, string TIN, bool Deleted, string CompanyType)
    {
        public Company ToModel(AddressDto address) =>
            CompanyType switch
            {
                "PartnerCompany" => new PartnerCompany(new(ExternalId), Name, TIN, address.ToModel()),
                _ => new OwnedCompany(new(ExternalId), Name, TIN, address.ToModel())
            };
    }

    private record AddressDto(int Id, Guid ExternalId, string StreetAddress, string City, string State, string PostalCode, string Country, int AddressKind)
    {
        public Address ToModel() =>
            new Address(new(ExternalId), StreetAddress, City, State, PostalCode, Country, (AddressKind)AddressKind);
    }

    public async Task<Company> AddAsync(Company entity, CancellationToken cancellationToken = default)
    {
        const string insertCompany = @"INSERT INTO business.Companies (Name, TIN, UserId, Deleted, ExternalId, CompanyType)
                                       OUTPUT INSERTED.Id
                                       VALUES (@Name, @TIN, @UserId, 0, @ExternalId, @CompanyType);";
        var companyId = await _connection.ExecuteScalarAsync<int>(
            insertCompany,
            new { entity.Name, entity.TIN, UserId = _userId.Value, ExternalId = entity.ExternalId.Value, CompanyType = entity.GetType().Name },
            transaction: _transaction
        );

        var _ = await AddAddress(entity, companyId);

        return entity;
    }

    private async Task<int> AddAddress(Company entity, int companyId)
    {
        const string insertAddress = @"INSERT INTO business.Addresses (CompanyId, StreetAddress, City, State, PostalCode, Country, ExternalId, AddressKind)
                                       OUTPUT INSERTED.Id
                                       VALUES (@CompanyId, @StreetAddress, @City, @State, @PostalCode, @Country, @ExternalId, @AddressKind);";
        var addressId = await _connection.ExecuteScalarAsync<int>(
            insertAddress,
            new
            {
                CompanyId = companyId,
                entity.Address.StreetAddress,
                entity.Address.City,
                entity.Address.State,
                entity.Address.PostalCode,
                entity.Address.Country,
                ExternalId = entity.Address.ExternalId.Value,
                AddressKind = (int)entity.Address.AddressKind
            },
            transaction: _transaction
        );
        return addressId;
    }

    public async Task UpdateAsync(Company entity, CancellationToken cancellationToken = default)
    {
        // Update Company
        const string updateCompany = @"UPDATE business.Companies
                                       SET Name = @Name, TIN = @TIN, ExternalId = @ExternalId
                                       WHERE ExternalId = @ExternalId AND UserId = @UserId AND Deleted = 0 AND CompanyType = @CompanyType;";
        var companyRows = await _connection.ExecuteAsync(
            updateCompany,
            new { entity.Name, entity.TIN, ExternalId = entity.ExternalId.Value, UserId = _userId.Value, CompanyType = entity.GetType().Name },
            transaction: _transaction
        );

        if (companyRows > 0) await UpdateAddress(entity);
    }

    private async Task UpdateAddress(Company entity)
    {
        const string updateAddress = @"UPDATE business.Addresses
                                       SET StreetAddress = @StreetAddress, City = @City,
                                           State = @State, PostalCode = @PostalCode, Country = @Country, ExternalId = @ExternalId, AddressKind = @AddressKind
                                       WHERE ExternalId = @ExternalId";
        await _connection.ExecuteAsync(
            updateAddress,
            new
            {
                entity.Address.StreetAddress,
                entity.Address.City,
                entity.Address.State,
                entity.Address.PostalCode,
                entity.Address.Country,
                ExternalId = entity.Address.ExternalId.Value,
                AddressKind = (int)entity.Address.AddressKind
            },
            transaction: _transaction
        );
    }

    public async Task DeleteAsync(ExternalId<Company> externalId, CancellationToken cancellationToken = default)
    {
        // Soft delete: set Deleted = 1
        const string softDeleteCompany =
            @"UPDATE business.Companies
            SET Deleted = 1
            WHERE ExternalId = @ExternalId AND UserId = @UserId;";
        await _connection.ExecuteAsync(softDeleteCompany, new { ExternalId = externalId.Value, UserId = _userId.Value }, transaction: _transaction);
    }
}
