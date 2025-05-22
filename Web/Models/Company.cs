namespace Web.Models;

public record Company(
    int Id,
    string Name,
    string TIN,
    Address Address
);
