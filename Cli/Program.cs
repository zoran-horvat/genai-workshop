using Spectre.Console.Cli;
using Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Authentication;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Register MigrationManager as a service
services.AddTransient(provider =>
    new Web.Data.MigrationManager(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.")));

// Register ResetDatabase command
services.AddTransient<ResetDatabase>();
// Register ListUsers command
services.AddTransient<ListUsers>();

// Register Data services
services.AddSingleton<Cli.Data.StreetAddressGenerator>();
services.AddSingleton<Cli.Data.CompanyNameGenerator>();
services.AddSingleton<Cli.Data.AddressGenerator>();
services.AddSingleton<Cli.Data.CompanyGenerator>();
services.AddTransient<Cli.Data.Companies>(provider =>
    new Cli.Data.Companies(
        provider.GetRequiredService<Cli.Data.CompanyGenerator>(),
        user =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;
            return new Web.Data.UnitOfWork(connectionString, new Web.Data.Abstractions.UserId(user.Id));
        }
    ));

// Register IConfiguration as a singleton service
services.AddSingleton<IConfiguration>(configuration);

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Create a type registrar for Spectre.Console.Cli
var registrar = new TypeRegistrar(serviceProvider);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.AddCommand<ResetDatabase>("resetdb");
    config.AddCommand<ListUsers>("list-users");
});

app.Run(args);

