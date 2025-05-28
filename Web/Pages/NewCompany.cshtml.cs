using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Web.Models;
using Web.Data.Abstractions;
using Web.Data;

namespace Web.Pages;

[Authorize(Roles = "User")]
public class NewCompanyModel : PageModel
{
    [BindProperty]
    [Required]
    public string CompanyName { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string TIN { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string StreetAddress { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string City { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string State { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string PostalCode { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string Country { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public List<AddressKind> SelectedAddressKinds { get; set; } = new();

    public List<AddressKind> AllAddressKinds { get; } = Enum.GetValues(typeof(AddressKind))
        .Cast<AddressKind>()
        .Where(k => k != AddressKind.Default && k != 0)
        .ToList();

    private readonly IUnitOfWork _unitOfWork;

    public NewCompanyModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [BindProperty(SupportsGet = true)]
    public string? kind { get; set; }

    public void OnGet()
    {
        // Optionally preselect defaults
        SelectedAddressKinds = new List<AddressKind> { AddressKind.Headquarter };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        // Combine selected kinds into a single AddressKind value
        var addressKind = SelectedAddressKinds.Any()
            ? SelectedAddressKinds.Aggregate((a, b) => a | b)
            : AddressKind.Default;

        var address = AddressFactory.CreateNew(StreetAddress, City, State, PostalCode, Country, addressKind);
        var company = (kind?.Equals("Partner", StringComparison.OrdinalIgnoreCase) == true)
            ? CompanyFactory.CreateNewPartner(CompanyName, TIN, address)
            : CompanyFactory.CreateNewOwned(CompanyName, TIN, address);

        await _unitOfWork.Companies.AddAsync(company);
        await _unitOfWork.CommitAsync();

        return RedirectToPage("/Companies");
    }
}
