using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Data.Abstractions;
using Web.Models;

namespace Web.Pages;

public class EditAddressModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public EditAddressModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [BindProperty]
    public EditAddressInputModel EditAddress { get; set; } = new();

    public Company Company { get; set; } = default!;
    public Address Address { get; set; } = default!;
    public List<SelectListItem> AddressKindList { get; set; } = new();

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid companyId, Guid addressId)
    {
        var company = await _unitOfWork.Companies.TryFindAsync(new ExternalId<Company>(companyId));
        if (company == null)
        {
            ErrorMessage = "Company not found.";
            return RedirectToPage("/Companies");
        }
        var address = company.Addresses?.FirstOrDefault(a => a.ExternalId.Value == addressId);
        if (address == null)
        {
            ErrorMessage = "Address not found.";
            return RedirectToPage("/EditCompany", new { id = companyId });
        }
        Company = company;
        Address = address;
        EditAddress = EditAddressInputModel.FromAddress(address);

        AddressKindList = Enum.GetValues(typeof(AddressKind))
            .Cast<AddressKind>()
            .Where(k => k != AddressKind.Default && k != 0)
            .Select(k => new SelectListItem { Value = k.ToString(), Text = k.ToString() })
            .ToList();

        // Use EditAddress.SelectedAddressKinds for binding
        EditAddress.SelectedAddressKinds = address.AddressKind.ToFlags()
            .Select(k => k.ToString())
            .ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid companyId, Guid addressId)
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(companyId, addressId); // repopulate data
            return Page();
        }
        var company = await _unitOfWork.Companies.TryFindAsync(new ExternalId<Company>(companyId));
        if (company == null)
        {
            ErrorMessage = "Company not found.";
            return RedirectToPage("/Companies");
        }
        var address = company.Addresses?.FirstOrDefault(a => a.ExternalId.Value == addressId);
        if (address == null)
        {
            ErrorMessage = "Address not found.";
            return RedirectToPage("/EditCompany", new { id = companyId });
        }

        var addressKind = EditAddress.SelectedAddressKinds
            .Select(kindStr => Enum.TryParse<AddressKind>(kindStr, out var kind) ? kind : (AddressKind)0)
            .Where(kind => kind != AddressKind.Default && kind != 0)
            .Aggregate((AddressKind)0, (acc, kind) => acc | kind);

        var updatedAddress = address with
        {
            StreetAddress = EditAddress.StreetAddress,
            City = EditAddress.City,
            State = EditAddress.State,
            PostalCode = EditAddress.PostalCode,
            Country = EditAddress.Country,
            AddressKind = addressKind
        };
        var updatedCompany = company.WithAddress(updatedAddress);
        await _unitOfWork.Companies.UpdateAsync(updatedCompany);
        await _unitOfWork.CommitAsync();
        return RedirectToPage("/EditCompany", new { id = company.ExternalId.Value });
    }

    public class EditAddressInputModel
    {
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string AddressKind { get; set; } = string.Empty;
        public List<string> SelectedAddressKinds { get; set; } = new();
        public static EditAddressInputModel FromAddress(Address address) => new()
        {
            StreetAddress = address.StreetAddress,
            City = address.City,
            State = address.State,
            PostalCode = address.PostalCode,
            Country = address.Country,
            AddressKind = address.AddressKind.ToString(),
            SelectedAddressKinds = address.AddressKind.ToFlags().Select(k => k.ToString()).ToList()
        };
    }
}
