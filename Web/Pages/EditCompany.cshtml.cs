using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Data.Abstractions;
using Web.Models;

namespace Web.Pages;

[Authorize(Roles = "User")]
public class EditCompanyModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public EditCompanyModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [BindProperty]
    public EditCompanyInputModel Company { get; set; } = default!;

    [TempData]
    public string? ErrorMessage { get; set; }

    public List<Address> CompanyAddresses { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var company = await _unitOfWork.Companies.TryFindAsync(new ExternalId<Company>(id));
        if (company == null)
        {
            ErrorMessage = "Company not found.";
            return RedirectToPage("/Companies");
        }

        Company = EditCompanyInputModel.FromCompany(company);
        CompanyAddresses = company.Addresses?.ToList() ?? new List<Address>();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var company = await _unitOfWork.Companies.TryFindAsync(new ExternalId<Company>(Company.Id));
        if (company == null)
        {
            ErrorMessage = "Company not found.";
            return RedirectToPage("/Companies");
        }

        // Check if a deleteAddressId was posted
        var deleteAddressIdStr = Request.Form["deleteAddressId"].FirstOrDefault();
        if (!string.IsNullOrEmpty(deleteAddressIdStr) && Guid.TryParse(deleteAddressIdStr, out var deleteAddressId))
        {
            // Remove the address from the company
            var updatedAddresses = company.Addresses.Where(a => a.ExternalId.Value != deleteAddressId).ToArray();
            var updated = company with
            {
                Name = Company.Name,
                TIN = Company.TIN,
                Addresses = updatedAddresses
            };
            await _unitOfWork.Companies.UpdateAsync(updated);
            await _unitOfWork.CommitAsync();
            // Stay on the same page after delete
            return RedirectToPage(new { id = Company.Id });
        }

        var updatedCompany = company with
        {
            Name = Company.Name,
            TIN = Company.TIN
        };

        await _unitOfWork.Companies.UpdateAsync(updatedCompany);
        await _unitOfWork.CommitAsync();

        return RedirectToPage("/Companies");
    }

    public class EditCompanyInputModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string TIN { get; set; } = "";

        public static EditCompanyInputModel FromCompany(Company company) => new()
        {
            Id = company.ExternalId.Value,
            Name = company.Name,
            TIN = company.TIN
        };
    }
}
