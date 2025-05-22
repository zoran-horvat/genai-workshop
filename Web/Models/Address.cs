namespace Web.Models;

public record Address(
    int Id,
    string StreetAddress,
    string City,
    string State,
    string PostalCode,
    string Country
);
