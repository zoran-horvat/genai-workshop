using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using Authentication;

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
}

