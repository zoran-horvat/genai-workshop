using Web.Data.Abstractions;

namespace Web.Models;

public record Address(
    ExternalId<Address> ExternalId, string StreetAddress,
    string City, string State, string PostalCode, string Country);

public static class AddressFactory
{
    public static Address CreateNew(string streetAddress, string city, string state, string postalCode, string country) =>
        new Address(ExternalId<Address>.CreateNew(), streetAddress, city, state, postalCode, country);

    public static Address CreateExisting(
        ExternalId<Address> externalId, string streetAddress, string city,
        string state, string postalCode, string country) =>
        new Address(externalId, streetAddress, city, state, postalCode, country);
}