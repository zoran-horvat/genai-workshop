﻿using Spectre.Console.Cli;
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

