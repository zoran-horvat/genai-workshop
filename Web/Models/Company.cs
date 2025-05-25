using Web.Data.Abstractions;

namespace Web.Models;

public record Company(ExternalId<Company> ExternalId, string Name, string TIN, Address Address);

public static class CompanyFactory
{
    public static Company CreateNew(string name, string tin, Address address) =>
        new Company(ExternalId<Company>.CreateNew(), name, tin, address);

    public static Company CreateExisting(ExternalId<Company> externalId, string name, string tin, Address address) =>
        new Company(externalId, name, tin, address);
}
