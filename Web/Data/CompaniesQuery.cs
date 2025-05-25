using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Web.Data.Abstractions;
using Web.ViewModels;

namespace Web.Data;

public class CompaniesQuery(SqlConnection connection, UserId userId) : IQuery<CompanyViewModel>
{
    private readonly IDbConnection _connection = connection;
    private readonly UserId _userId = userId.EnsureNonEmpty();

    public async Task<IEnumerable<CompanyViewModel>> GetAllAsync()
    {
        var sql = @"SELECT c.ExternalId AS CompanyId, c.Name, c.TIN, a.Id AS AddressId, a.StreetAddress, a.City, a.State, a.PostalCode, a.Country
                    FROM business.Companies c
                    INNER JOIN business.Addresses a ON a.CompanyId = c.Id
                    WHERE c.UserId = @UserId AND c.Deleted = 0";

        var companies = await _connection.QueryAsync<CompanyDto, AddressDto, CompanyViewModel>(
            sql,
            (company, address) => company.ToViewModel(address),
            param: new { UserId = _userId.Value },
            splitOn: "AddressId"
        );

        return companies;
    }

    private record CompanyDto(Guid CompanyId, string Name, string TIN)
    {
        public CompanyViewModel ToViewModel(AddressDto address) =>
            new(CompanyId, Name, TIN, address.ToViewModel());
    }

    private record AddressDto(int AddressId, string StreetAddress, string City, string State, string PostalCode, string Country)
    {
        public AddressViewModel ToViewModel() =>
            new(AddressId, StreetAddress, City, State, PostalCode, Country);
    }
}