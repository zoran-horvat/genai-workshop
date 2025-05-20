using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Commands;

public class ListUsers : Command<ListUsers.Settings>
{
    private readonly ApplicationDbContext _dbContext;

    public ListUsers(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public sealed class Settings : CommandSettings { }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var users = _dbContext.Users.ToList();
        var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(_dbContext), null!, new PasswordHasher<ApplicationUser>(),
            new List<IUserValidator<ApplicationUser>> { new UserValidator<ApplicationUser>() },
            new List<IPasswordValidator<ApplicationUser>> { new PasswordValidator<ApplicationUser>() },
            null!, null!, null!, null!);

        var table = new Table();
        table.AddColumn(new TableColumn("#"));
        table.AddColumn(new TableColumn("User name"));
        table.AddColumn(new TableColumn("Role(s)"));

        int idx = 1;
        foreach (var user in users.OrderBy(u => u.UserName))
        {
            var userName = user.UserName ?? string.Empty;
            var roles = userManager.GetRolesAsync(user).GetAwaiter().GetResult() ?? new List<string>();
            string rolesStr = string.Join(", ", roles);
            table.AddRow(idx.ToString(), userName, rolesStr);
            idx++;
        }

        AnsiConsole.Write(table);
        return 0;
    }
}
