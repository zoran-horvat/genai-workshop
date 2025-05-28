namespace Web.Models;

public static class CompanyAddresses
{
    public static Company Add(this Company company, Address address) =>
        company with { Addresses = [.. company.Addresses, address] };

    public static Company WithAddress(this Company company, Address address) =>
        company with { Addresses = company.Addresses.WithAddress(address) };

    private static Address[] WithAddress(this Address[] addresses, Address address) =>
        addresses.Select(a => a.ExternalId == address.ExternalId ? address : a).ToArray();
}