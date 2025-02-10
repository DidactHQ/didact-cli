// See https://aka.ms/new-console-template for more information
using DidactCli.Commands;
using DidactCli.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

AnsiConsole.Write(
    new FigletText("Didact Cli")
        .LeftJustified()
        .Color(Color.Orange1));

#region Setup and bind the appsettings.

var iConfiguration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory) // Ensure correct file path
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();
var appSettings = new AppSettings();
iConfiguration.Bind(appSettings);

#endregion

#region Setup dependency injection and other bootstrapping utilities.

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(iConfiguration);
services.AddSingleton(appSettings);
services.AddLogging(configure =>
{
    configure.AddConsole();
});
var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

#endregion

app.Configure(config =>
{
    config.SetApplicationName("Didact Cli");
    config.AddCommand<VersionCommand>("version")
        .WithDescription("The version of Didact Cli.");

    config.AddBranch("engine", engine =>
    {
        engine.AddCommand<EngineInstallCommand>("install") // run "didact engine set" after installation? --set-after-install true ?
            .WithDescription("Installs Didact Engine to the target location.");
        engine.AddCommand<TestCommand>("set")
            .WithDescription("Sets the target Didact Engine.");
        engine.AddCommand<TestCommand>("get")
            .WithDescription("Gets the target Didact Engine.");
        engine.AddBranch("config", config =>
        {
            config.AddCommand<TestCommand>("generate") // --open true option for autoopening config file. // --with-file "" option for using existing config file.
                .WithDescription("Generates a new runtime environment variables file for Didact Engine.");
            config.AddCommand<TestCommand>("get")
                .WithDescription("Gets the runtime environment variables file for Didact Engine.");
        });
    });

    config.AddBranch("ui", ui =>
    {
        ui.AddCommand<UiInstallCommand>("install")
            .WithDescription("Installs Didact UI to the target location.");
        ui.AddCommand<TestCommand>("set")
            .WithDescription("Sets the target Didact UI.");
        ui.AddCommand<TestCommand>("get")
            .WithDescription("Gets the target Didact UI.");
        ui.AddBranch("config", config =>
        {
            config.AddCommand<TestCommand>("generate") // --open true option for autoopening config file.
                .WithDescription("Generates a new runtime environment variables file for Didact UI.");
            config.AddCommand<TestCommand>("get")
                .WithDescription("Gets the runtime environment variables file for Didact UI.");
        });
    });

    config.AddBranch("license", license =>
    {
        license.AddCommand<TestCommand>("set")
            .WithDescription("Sets the Didact license key.");
        license.AddCommand<TestCommand>("get")
            .WithDescription("Gets the Didact license key.");
        license.AddCommand<TestCommand>("authenticate")
            .WithDescription("Authenticates the Didact license key.");
        license.AddCommand<TestCommand>("validate")
            .WithDescription("Validates the Didact license key.");
    });

    config.AddBranch("library", library =>
    {
        library.AddCommand<TestCommand>("list")
            .WithDescription("Lists the registered Didact flow libraries.");
        library.AddCommand<TestCommand>("add")
            .WithDescription("Adds a Didact flow library to the registry.");
        library.AddCommand<TestCommand>("remove")
            .WithDescription("Removes a Didact flow library to the registry.");
        library.AddCommand<TestCommand>("deploy")
            .WithDescription("Deploys all flow libraries in the registry.");
    });
});

return app.Run(args);