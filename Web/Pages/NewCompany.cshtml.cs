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

    private readonly IUnitOfWork _unitOfWork;

    public NewCompanyModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var address = AddressFactory.CreateNew(StreetAddress, City, State, PostalCode, Country );
        var company = CompanyFactory.CreateNew(CompanyName, TIN, address);

        await _unitOfWork.Companies.AddAsync(company);
        await _unitOfWork.CommitAsync();

        return RedirectToPage("/Companies");
    }
}
