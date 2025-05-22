using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();
        // TODO: Save the new company for the logged-in user
        return RedirectToPage("/Companies");
    }
}
