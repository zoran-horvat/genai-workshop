namespace Web.ViewModels;

public record CompanyViewModel(Guid Id, string Name, string TIN, AddressViewModel Address);