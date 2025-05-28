using Web.Data.Abstractions;

namespace Web.Models;

public abstract record Company(ExternalId<Company> ExternalId, string Name, string TIN, Address[] Addresses);

public record OwnedCompany(ExternalId<Company> ExternalId, string Name, string TIN, Address[] Addresses)
    : Company(ExternalId, Name, TIN, Addresses);
public record PartnerCompany(ExternalId<Company> ExternalId, string Name, string TIN, Address[] Addresses)
    : Company(ExternalId, Name, TIN, Addresses);

public static class CompanyFactory
{
    public static Company CreateNewOwned(string name, string tin, Address address, params Address[] otherAddresses) =>
        new OwnedCompany(ExternalId<Company>.CreateNew(), name, tin, [address, ..otherAddresses]);

    public static Company CreateExistingOwned(ExternalId<Company> externalId, string name, string tin, Address address, params Address[] otherAddresses) =>
        new OwnedCompany(externalId, name, tin, [address, ..otherAddresses]);

    public static Company CreateNewPartner(string name, string tin, Address address, params Address[] otherAddresses) =>
        new PartnerCompany(ExternalId<Company>.CreateNew(), name, tin, [address, ..otherAddresses]);

    public static Company CreateExistingPartner(ExternalId<Company> externalId, string name, string tin, Address address, params Address[] otherAddresses) =>
        new PartnerCompany(externalId, name, tin, [address, ..otherAddresses]);
}
