using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Authentication;
using Web.Data.Abstractions;
using System.Security.Claims;
using Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

// Add Microsoft Identity services
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Register IHttpContextAccessor for accessing user info
builder.Services.AddHttpContextAccessor();

// Register IUnitOfWork
builder.Services.AddScoped<IUnitOfWork>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var httpContext = httpContextAccessor.HttpContext;

    UserId userId =
        httpContext?.User?.Identity?.IsAuthenticated == true &&
        httpContext.User.FindFirst(ClaimTypes.NameIdentifier) is Claim userIdClaim &&
        !string.IsNullOrEmpty(userIdClaim.Value)
            ? new UserId(userIdClaim.Value)
            : UserId.Empty;

    return new UnitOfWork(connectionString, userId);
});

// Register CompaniesQuery
builder.Services.AddScoped<CompaniesQuery>(provider =>
{
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var httpContext = httpContextAccessor.HttpContext;
    UserId userId =
        httpContext?.User?.Identity?.IsAuthenticated == true &&
        httpContext.User.FindFirst(ClaimTypes.NameIdentifier) is Claim userIdClaim &&
        !string.IsNullOrEmpty(userIdClaim.Value)
            ? new UserId(userIdClaim.Value)
            : UserId.Empty;
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    var dbConnection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
    return new CompaniesQuery(dbConnection, userId);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
