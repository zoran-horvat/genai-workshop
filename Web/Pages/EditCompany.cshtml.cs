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

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var company = await _unitOfWork.Companies.TryFindAsync(new EntityId<Company>(id));
        if (company == null)
        {
            ErrorMessage = "Company not found.";
            return RedirectToPage("/Companies");
        }

        Company = EditCompanyInputModel.FromCompany(company);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var company = await _unitOfWork.Companies.TryFindAsync(new EntityId<Company>(Company.Id));
        if (company == null)
        {
            ErrorMessage = "Company not found.";
            return RedirectToPage("/Companies");
        }

        var updated = company with
        {
            Name = Company.Name,
            TIN = Company.TIN,
            Address = company.Address with
            {
                StreetAddress = Company.Address.StreetAddress,
                City = Company.Address.City,
                State = Company.Address.State,
                PostalCode = Company.Address.PostalCode,
                Country = Company.Address.Country
            }
        };

        await _unitOfWork.Companies.UpdateAsync(updated);
        await _unitOfWork.CommitAsync();

        return RedirectToPage("/Companies");
    }

    public class EditCompanyInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string TIN { get; set; } = "";
        public EditAddressInputModel Address { get; set; } = new();

        public static EditCompanyInputModel FromCompany(Company company) => new()
        {
            Id = company.Id.Value,
            Name = company.Name,
            TIN = company.TIN,
            Address = new EditAddressInputModel
            {
                Id = company.Address.Id.Value,
                StreetAddress = company.Address.StreetAddress,
                City = company.Address.City,
                State = company.Address.State,
                PostalCode = company.Address.PostalCode,
                Country = company.Address.Country
            }
        };
    }

    public class EditAddressInputModel
    {
        public int Id { get; set; }
        public string StreetAddress { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string Country { get; set; } = "";
    }
}
