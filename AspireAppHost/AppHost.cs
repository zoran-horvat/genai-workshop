
const string connStringName = "DefaultConnection";

var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("SqlPassword", secret: false);

var sql = builder.AddSqlServer("sql")
                 .WithDataVolume()
                 .WithPassword(sqlPassword);

var database = sql.AddDatabase(connStringName);

var resetDatabase = builder.AddProject<Projects.Cli>("reset-database")
                 .WithReference(database)
                 .WithArgs("resetdb")
                 .WaitFor(database);

var listUsers = builder.AddProject<Projects.Cli>("list-users")
                 .WithReference(database)
                 .WithArgs("list-users")
                 .WaitFor(database)
                 .WaitForCompletion(resetDatabase);
                 

var web = builder.AddProject<Projects.Web>("web")
                 .WithReference(database)
                 .WaitFor(database)
                 .WaitForCompletion(resetDatabase);

await builder.Build().RunAsync();