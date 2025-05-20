using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Commands;

public class ResetDatabase : Command<ResetDatabase.Settings>
{
    private readonly ApplicationDbContext _dbContext;

    public ResetDatabase(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public sealed class Settings : CommandSettings
    {
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        Console.WriteLine("Resetting the database...");
        DropDatabase();
        RecreateDatabase();
        CreateTestUsersAndRoles();
        return 0;
    }

    private void DropDatabase()
    {
        _dbContext.Database.EnsureDeleted();
        Console.WriteLine("Database has been dropped successfully.");
    }

    private void RecreateDatabase()
    {
        _dbContext.Database.EnsureCreated();
        Console.WriteLine("Database has been recreated successfully.");
    }

    private void CreateTestUsersAndRoles()
    {
        var userManager = GetUserManager(allowWeakPasswords: true);
        var roleManager = GetRoleManager();

        CreateUserIfNotExists(userManager, roleManager, "owner", "owner@example.com", "owner", "Owner");
        CreateUserIfNotExists(userManager, roleManager, "user1", "user1@example.com", "user1", "User");
        CreateUserIfNotExists(userManager, roleManager, "user2", "user2@example.com", "user2", "User");
        CreateUserIfNotExists(userManager, roleManager, "user3", "user3@example.com", "user3", "User");

        // Revert to default password policy after test users are created
        userManager = GetUserManager(allowWeakPasswords: false);
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

    private void CreateUserIfNotExists(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, string username, string email, string password, string role)
    {
        var user = userManager.FindByNameAsync(username).GetAwaiter().GetResult();
        if (user == null)
        {
            user = new ApplicationUser { UserName = username, Email = email, EmailConfirmed = true };
            var result = userManager.CreateAsync(user, password).GetAwaiter().GetResult();
            if (!result.Succeeded)
            {
                Console.WriteLine($"Failed to create user {username}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                return;
            }
            Console.WriteLine($"Created user: {username}");
        }
        else
        {
            Console.WriteLine($"User already exists: {username}");
        }
        if (!userManager.IsInRoleAsync(user, role).GetAwaiter().GetResult())
        {
            var roleResult = userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
            if (!roleResult.Succeeded)
            {
                Console.WriteLine($"Failed to add user {username} to role {role}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
            else
            {
                Console.WriteLine($"Added user {username} to role {role}");
            }
        }
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