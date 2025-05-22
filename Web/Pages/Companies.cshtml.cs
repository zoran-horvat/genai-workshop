using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Models;
using System.Security.Claims;

namespace Web.Pages;

[Authorize(Roles = "User")]
public class CompaniesModel : PageModel
{
    public List<Company> Companies { get; set; } = new();

    public void OnGet()
    {
        // TODO: Replace with real data access
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Example: Fetch companies for the logged-in user
        Companies = new List<Company>();
    }
}
