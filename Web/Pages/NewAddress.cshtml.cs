using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Data.Abstractions;
using Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Pages;

public class NewAddressModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public NewAddressModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [BindProperty]
    public NewAddressInputModel NewAddress { get; set; } = new();

    public Company Company { get; set; } = default!;
    public List<Address> CompanyAddresses { get; set; } = new();
    public List<SelectListItem> AddressKindList { get; set; } = new();

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var company = await _unitOfWork.Companies.TryFindAsync(new ExternalId<Company>(id));
        if (company == null)
        {
            ErrorMessage = "Company not found.";
            return RedirectToPage("/Companies");
        }
        Company = company;
        CompanyAddresses = company.Addresses?.ToList() ?? new List<Address>();
        AddressKindList = Enum.GetValues(typeof(AddressKind))
            .Cast<AddressKind>()
            .Where(k => k != AddressKind.Default && k != 0)
            .Select(k => new SelectListItem { Value = k.ToString(), Text = k.ToString() })
            .ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(id); // repopulate data
            return Page();
        }
        var company = await _unitOfWork.Companies.TryFindAsync(new ExternalId<Company>(id));
        if (company == null)
        {
            ErrorMessage = "Company not found.";
            return RedirectToPage("/Companies");
        }

        var addressKind = AddressKind.Default;

        if (SelectedAddressKinds.Count > 0)
            addressKind = SelectedAddressKinds
                .Select(kindStr => Enum.TryParse<AddressKind>(kindStr, out var kind) ? kind : AddressKind.Default)
                .Aggregate((AddressKind)0, (acc, kind) => acc | kind);

        var newAddress = new Address(
            ExternalId<Address>.CreateNew(),
            NewAddress.StreetAddress,
            NewAddress.City,
            NewAddress.State,
            NewAddress.PostalCode,
            NewAddress.Country,
            addressKind
        );

        var updatedCompany = company with { Addresses = [..company.Addresses, newAddress] };
        await _unitOfWork.Companies.UpdateAsync(updatedCompany);
        await _unitOfWork.CommitAsync();
        return RedirectToPage("/EditCompany", new { id = company.ExternalId.Value });
    }

    public class NewAddressInputModel
    {
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string AddressKind { get; set; } = string.Empty;
        public List<string> SelectedAddressKinds { get; set; } = new();
    }

    [BindProperty]
    public List<string> SelectedAddressKinds { get; set; } = new();
}
