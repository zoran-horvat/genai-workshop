using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Web.Data.Abstractions;
using Web.Models;
using Web.ViewModels;

namespace Web.Data;

public class CompaniesQuery(SqlConnection connection, UserId userId) : IQuery<CompanyViewModel>
{
    private readonly IDbConnection _connection = connection;
    private readonly UserId _userId = userId.EnsureNonEmpty();

    public async Task<IEnumerable<CompanyViewModel>> GetAllAsync()
    {
        var sql = @"SELECT c.ExternalId AS CompanyId, c.Name, c.TIN, c.CompanyType, a.Id AS AddressId, a.StreetAddress, a.City, a.State, a.PostalCode, a.Country, a.AddressKind
                    FROM business.Companies c
                    INNER JOIN business.Addresses a ON a.CompanyId = c.Id
                    WHERE c.UserId = @UserId AND c.Deleted = 0";

        var companyDict = new Dictionary<Guid, CompanyDto>();

        await _connection.QueryAsync<CompanyDto, AddressDto, CompanyDto>(
            sql,
            (company, address) =>
            {
                if (!companyDict.TryGetValue(company.CompanyId, out var companyEntry))
                {
                    companyEntry = company with { Addresses = new List<AddressDto>() };
                    companyDict.Add(company.CompanyId, companyEntry);
                }
                companyEntry.Addresses.Add(address);
                return companyEntry;
            },
            param: new { UserId = _userId.Value },
            splitOn: "AddressId"
        );

        return companyDict.Values.Select(c => c.ToViewModel());
    }

    private record CompanyDto(Guid CompanyId, string Name, string TIN, string CompanyType)
    {
        public List<AddressDto> Addresses { get; init; } = new();

        public CompanyViewModel ToViewModel() =>
            new(CompanyId, ToCompanyKind(CompanyType), Name, TIN, Addresses.Select(a => a.ToViewModel()).ToArray());

        private static CompanyViewModel.CompanyKind ToCompanyKind(string companyType) => companyType switch
        {
            "PartnerCompany" => CompanyViewModel.CompanyKind.Partner,
            _ => CompanyViewModel.CompanyKind.Owned
        };
    }

    private record AddressDto(int AddressId, string StreetAddress, string City, string State, string PostalCode, string Country, int AddressKind)
    {
        public AddressViewModel ToViewModel() =>
            new(AddressId, StreetAddress, City, State, PostalCode, Country, (AddressKind)AddressKind);
    }
}