using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Web.Data;
using Cli.Data;
using Spectre.Console;

namespace Commands;

public class ResetDatabase(ApplicationDbContext dbContext, MigrationManager migrationManager, Companies companies) : AsyncCommand<ResetDatabase.Settings>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly MigrationManager _migrationManager = migrationManager;
    private readonly Companies _companies = companies;

    public sealed class Settings : CommandSettings
    {
    }

    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        Console.WriteLine("Resetting the database...");
        await DropDatabase();
        await RecreateDatabase();
        await Task.Run(() => _migrationManager.ApplyMigrations());
    
        List<ApplicationUser> users = new();
        await foreach (var user in CreateTestUsersAndRolesAsync()) users.Add(user);

        await CreateTestCompanies(users);
        return 0;
    }

    private async Task DropDatabase()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        Console.WriteLine("Database has been dropped successfully.");
    }

    private async Task RecreateDatabase()
    {
        await _dbContext.Database.EnsureCreatedAsync();
        Console.WriteLine("Database has been recreated successfully.");
    }

    private async Task CreateTestCompanies(IEnumerable<ApplicationUser> users)
    {
        foreach (var user in users)
        {
            await foreach (var (_, company) in _companies.CreateCompanies(user))
            {
                AnsiConsole.MarkupLine(
                    $"Created company: [white][bold]{user.UserName}[/][/] {company.GetType().Name.Replace("Company", "")} " +
                    $"[white][bold]{company.Name}[/][/] {company.Addresses[0].StreetAddress} " +
                    $"{company.Addresses[0].City}, {company.Addresses[0].Country}");
            }
        }
    }

    private async IAsyncEnumerable<ApplicationUser> CreateTestUsersAndRolesAsync()
    {
        var userManager = GetUserManager(allowWeakPasswords: true);
        var roleManager = GetRoleManager();

        var users = new List<ApplicationUser>();
        var userInfos = new[]
        {
            ("owner", "owner@example.com", "owner", "Owner"),
            ("user1", "user1@example.com", "user1", "User"),
            ("user2", "user2@example.com", "user2", "User"),
            ("user3", "user3@example.com", "user3", "User")
        };

        foreach (var (username, email, password, role) in userInfos)
        {
            if (await CreateUserIfNotExistsAsync(userManager, roleManager, username, email, password, role) is ApplicationUser user)
            {
                users.Add(user);
                yield return user;
            }
        }
        userManager = GetUserManager(allowWeakPasswords: false);
    }

    private async Task<ApplicationUser?> CreateUserIfNotExistsAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, string username, string email, string password, string role)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user == null)
        {
            user = new ApplicationUser { UserName = username, Email = email, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                Console.WriteLine($"Failed to create user {username}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            Console.WriteLine($"Created user: {username}");
        }
        else
        {
            Console.WriteLine($"User already exists: {username}");
        }
        if (!await userManager.IsInRoleAsync(user, role))
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                Console.WriteLine($"Failed to add user {username} to role {role}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
            else
            {
                Console.WriteLine($"Added user {username} to role {role}");
            }
        }
        return user;
    }

    private UserManager<ApplicationUser> GetUserManager(bool allowWeakPasswords = false)
    {
        var store = new UserStore<ApplicationUser>(_dbContext);
        var hasher = new PasswordHasher<ApplicationUser>();
        var userValidators = new List<IUserValidator<ApplicationUser>> { new UserValidator<ApplicationUser>() };
        var pwdValidators = new List<IPasswordValidator<ApplicationUser>>();
        if (allowWeakPasswords)
        {
            pwdValidators.Add(new AllowAllPasswordValidator<ApplicationUser>());
        }
        else
        {
            pwdValidators.Add(new PasswordValidator<ApplicationUser>());
        }
        return new UserManager<ApplicationUser>(store, null!, hasher, userValidators, pwdValidators, null!, null!, null!, null!);
    }

    private RoleManager<IdentityRole> GetRoleManager()
    {
        var store = new RoleStore<IdentityRole>(_dbContext);
        var validators = new List<IRoleValidator<IdentityRole>> { new RoleValidator<IdentityRole>() };
        return new RoleManager<IdentityRole>(store, validators, null!, null!, null!);
    }
}

// Add AllowAllPasswordValidator class
public class AllowAllPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
{
    public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string? password)
    {
        return Task.FromResult(IdentityResult.Success);
    }
}