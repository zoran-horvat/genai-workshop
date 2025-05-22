using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Web.Data;

public class MigrationManager
{
    private readonly string _connectionString;
    private readonly Assembly _assembly;

    public MigrationManager(string connectionString)
    {
        _connectionString = connectionString;
        _assembly = Assembly.GetExecutingAssembly();
    }

    public void InitializeMigrations()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Migrations')
BEGIN
    CREATE TABLE dbo.Migrations (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        MigrationNumber INT NOT NULL UNIQUE,
        MigrationName NVARCHAR(200) NOT NULL,
        AppliedOn DATETIME NOT NULL DEFAULT GETDATE()
    );
END";
        cmd.ExecuteNonQuery();
    }

    public void ApplyMigrations()
    {
        InitializeMigrations();

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        int lastMigration = GetLastAppliedMigrationNumber(connection);
        var migrationResources = GetMigrationResources();

        foreach (var res in migrationResources.Where(r => r.Number > lastMigration))
        {
            ApplyMigration(res, connection);
        }
    }

    private void ApplyMigration(MigrationResource migration, SqlConnection connection)
    {
        var sql = GetMigrationSql(migration);

        using var tx = connection.BeginTransaction();
        try
        {
            var migrationCmd = connection.CreateCommand();
            migrationCmd.Transaction = tx;
            migrationCmd.CommandText = sql;
            migrationCmd.ExecuteNonQuery();

            var insertCmd = connection.CreateCommand();
            insertCmd.Transaction = tx;
            insertCmd.CommandText = "INSERT INTO dbo.Migrations (MigrationNumber, MigrationName, AppliedOn) VALUES (@num, @name, GETDATE())";
            insertCmd.Parameters.AddWithValue("@num", migration.Number);
            insertCmd.Parameters.AddWithValue("@name", migration.Name);
            
            insertCmd.ExecuteNonQuery();

            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    private record MigrationResource(int Number, string Name);

    private IEnumerable<MigrationResource> GetMigrationResources() =>
        _assembly.GetManifestResourceNames()
            .Where(r => r.Contains("Migrations.migration-") && r.EndsWith(".sql"))
            .Select(ToMigrationResource)
            .OrderBy(r => r.Number);

    private MigrationResource ToMigrationResource(string resourceName) =>
        Regex.Matches(resourceName, @"migration-(\d+)\.sql")
            .Select(m => int.Parse(m.Groups[1].Value))
            .Select(n => new MigrationResource(n, resourceName))
            .First();

    private int GetLastAppliedMigrationNumber(SqlConnection connection)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT ISNULL(MAX(MigrationNumber), 0) FROM dbo.Migrations";
        return (int)cmd.ExecuteScalar();
    }

    private string GetMigrationSql(MigrationResource resource)
    {
        using var stream = _assembly.GetManifestResourceStream(resource.Name);
        using var reader = new StreamReader(stream!);
        return reader.ReadToEnd();
    }
}