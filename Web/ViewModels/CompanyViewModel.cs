namespace Web.ViewModels;

public record CompanyViewModel(
    Guid Id, CompanyViewModel.CompanyKind Kind,
    string Name, string TIN, AddressViewModel[] Addresses)
{
    public enum CompanyKind { Owned, Partner };
}