using Web.Data.Abstractions;

namespace Web.Models;

public record Address(
    EntityId<Address> Id, string StreetAddress, string City,
    string State, string PostalCode, string Country)
{
    public static Address CreateNew(
        string streetAddress, string city,
        string state, string postalCode, string country) =>
        new Address(EntityId<Address>.Empty, streetAddress, city, state, postalCode, country);

    public static Address CreateExisting(
        EntityId<Address> id, string streetAddress, string city,
        string state, string postalCode, string country) =>
        new Address(id, streetAddress, city, state, postalCode, country);

    public Address SetId(EntityId<Address> id) => this with { Id = id };
}
