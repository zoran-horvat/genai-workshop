using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Data;
using Web.Data.Abstractions;
using Web.Models;
using Web.ViewModels;

namespace Web.Pages;

[Authorize(Roles = "User")]
public class CompaniesModel : PageModel
{
    public List<CompanyViewModel> Companies { get; set; } = new();

    private readonly CompaniesQuery _companiesQuery;
    private readonly IUnitOfWork _unitOfWork;

    public CompaniesModel(CompaniesQuery companiesQuery, IUnitOfWork unitOfWork)
    {
        _companiesQuery = companiesQuery;
        _unitOfWork = unitOfWork;
    }

    public async Task OnGetAsync()
    {
        Companies = (await _companiesQuery.GetAllAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(int? deleteId)
    {
        if (deleteId.HasValue)
        {
            await _unitOfWork.Companies.DeleteAsync(new EntityId<Company>(deleteId.Value));
            await _unitOfWork.CommitAsync();
        }
        return RedirectToPage();
    }
}
