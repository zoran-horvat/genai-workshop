using Web.Data.Abstractions;

namespace Web.Models;

public record Company(EntityId<Company> Id, string Name, string TIN, Address Address)
{
    public static Company CreateNew(string name, string tin, Address address) =>
        address.Id.IsEmpty ? new Company(EntityId<Company>.Empty, name, tin, address)
        : throw new InvalidOperationException("Cannot create a new company with an existing address.");

    public static Company CreateExisting(EntityId<Company> id, string name, string tin, Address address) =>
        !address.Id.IsEmpty ? new Company(id, name, tin, address)
        : throw new InvalidOperationException("Cannot create an existing company with an empty address.");

    public Company SetId(EntityId<Company> id) => this with { Id = id };
}
