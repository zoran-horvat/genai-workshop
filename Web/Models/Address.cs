using Web.Data.Abstractions;

namespace Web.Models;

public record Address(
    ExternalId<Address> ExternalId, string StreetAddress,
    string City, string State, string PostalCode, string Country,
    AddressKind AddressKind = AddressKind.Default);

public static class AddressFactory
{
    public static Address CreateNew(
        string streetAddress, string city, string state,
        string postalCode, string country, AddressKind addressKind) =>
        new Address(ExternalId<Address>.CreateNew(), streetAddress, city, state, postalCode, country, addressKind);

    public static Address CreateExisting(
        ExternalId<Address> externalId, string streetAddress, string city,
        string state, string postalCode, string country, AddressKind addressKind) =>
        new Address(externalId, streetAddress, city, state, postalCode, country, addressKind);
}