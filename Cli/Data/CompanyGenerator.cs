using Web.Data.Abstractions;
using Web.Models;

namespace Cli.Data;

public class CompanyGenerator(CompanyNameGenerator companyNameGenerator, AddressGenerator addressGenerator)
{
    private readonly CompanyNameGenerator _companyNameGenerator = companyNameGenerator;
    private readonly AddressGenerator _addressGenerator = addressGenerator;
    private readonly Random _random = new(42);

    public Company Next(int maxAddresses = 1, params Type[] companyTypes)
    {
        if (companyTypes == null || companyTypes.Length == 0)
            throw new ArgumentException("At least one company type must be specified.");

        var type = companyTypes[_random.Next(companyTypes.Length)];
        var name = _companyNameGenerator.Next();
        var tin = _random.Next(10_000_000, 100_000_000).ToString();
        var addresses = GenerateAddresses(maxAddresses, type).ToArray();

        if (type == typeof(OwnedCompany))
            return new OwnedCompany(ExternalId<Company>.CreateNew(), name, tin, addresses);
        if (type == typeof(PartnerCompany))
            return new PartnerCompany(ExternalId<Company>.CreateNew(), name, tin, addresses);
        throw new ArgumentException($"Unsupported company type: {type}");
    }

    private IEnumerable<Address> GenerateAddresses(int maxAddresses, Type companyType)
    {
        int generate = _random.Next(1, maxAddresses + 1);
        int generated = 0;
        bool containsBilling = false;
        while (generated < generate || (companyType == typeof(PartnerCompany) && !containsBilling))
        {
            var address = _addressGenerator.Next();
            containsBilling = containsBilling || address.AddressKind.HasFlag(AddressKind.Billing);
            yield return address;
            generated++;
        }
    }

    public IEnumerable<Company> Companies(int maxAddresses, params Type[] companyTypes)
    {
        while (true)
            yield return Next(maxAddresses, companyTypes);
    }
}
