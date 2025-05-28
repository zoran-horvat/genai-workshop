using Web.Data.Abstractions;

namespace Web.Models;

public abstract record Company(ExternalId<Company> ExternalId, string Name, string TIN, Address Address);

public record OwnedCompany(ExternalId<Company> ExternalId, string Name, string TIN, Address Address)
    : Company(ExternalId, Name, TIN, Address);
public record PartnerCompany(ExternalId<Company> ExternalId, string Name, string TIN, Address Address)
    : Company(ExternalId, Name, TIN, Address);

public static class CompanyFactory
{
    public static Company CreateNewOwned(string name, string tin, Address address) =>
        new OwnedCompany(ExternalId<Company>.CreateNew(), name, tin, address);

    public static Company CreateExistingOwned(ExternalId<Company> externalId, string name, string tin, Address address) =>
        new OwnedCompany(externalId, name, tin, address);

    public static Company CreateNewPartner(string name, string tin, Address address) =>
        new PartnerCompany(ExternalId<Company>.CreateNew(), name, tin, address);

    public static Company CreateExistingPartner(ExternalId<Company> externalId, string name, string tin, Address address) =>
        new PartnerCompany(externalId, name, tin, address);
}
