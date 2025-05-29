using System.Text.RegularExpressions;
using Authentication;
using Web.Models;
using Web.Data.Abstractions;

namespace Cli.Data;

public class Companies(CompanyGenerator companyGenerator, Func<ApplicationUser, IUnitOfWork> uowFactory)
{
    private readonly CompanyGenerator _companyGenerator = companyGenerator;
    private readonly Func<ApplicationUser, IUnitOfWork> _uowFactory = uowFactory;

    private IEnumerable<Company> GenerateCompanies(ApplicationUser user) =>
        _companyGenerator.Companies(1, typeof(OwnedCompany)).Take(GetOwnedCompaniesCount(user))
            .Concat(_companyGenerator.Companies(3, typeof(PartnerCompany)).Take(GetPartnerCompaniesCount(user)));

    private int GetOwnedCompaniesCount(ApplicationUser user) => (1 << GetUserNumber(user)) - 1;

    private int GetPartnerCompaniesCount(ApplicationUser user) => (1 << (GetUserNumber(user) * 3)) - 1;

    private int GetUserNumber(ApplicationUser user) =>
        Regex.Match(user.UserName ?? string.Empty, @"(\d+)$") is Match match && match.Success
            ? int.Parse(match.Value)
            : 0;

    public async IAsyncEnumerable<(ApplicationUser user, Company company)> CreateCompanies(ApplicationUser user)
    {
        foreach (var company in GenerateCompanies(user))
        {
            using var uow = _uowFactory(user);
            await uow.Companies.AddAsync(company);
            await uow.CommitAsync();
            yield return (user, company);
        }
    }
}
