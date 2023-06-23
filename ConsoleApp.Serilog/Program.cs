using Microsoft.Extensions.Configuration;
// Implementation of key-value pair based configuration for Microsoft.Extensions.Configuration.
// Includes the memory configuration provider.
using Microsoft.Extensions.DependencyInjection;
// Default implementation of dependency injection for Microsoft.Extensions.DependencyInjection.
using Microsoft.Extensions.Hosting;
// Hosting and startup infrastructures for applications.
using Serilog;
// Simple .NET logging with fully-structured events.

namespace ConsoleApp.Serilog
{
    internal class Program
    {
        // Create a Method to ..
        // .. implement the file configuration provider (JSON)
        // .. implement the Environment Variables configuration provider
        /// <summary>
        /// Modifies a configuration object, using the file configuration provider (JSON) and the Environment Variables configuration provider.
        /// </summary>
        static void BuildConfig(IConfigurationBuilder configurationBuilder)
        {
            // Modify a configuration object, using the file configuration provider (JSON) and the Environment Variables configuration provider
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Build()).Enrich.FromLogContext().WriteTo.Console().CreateLogger();
            Log.Logger.Information("Starting ...");
            Log.Logger.Debug("Getting DOTNET_ENVIRONMENT: {DOTNET_ENVIRONMENT}", Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT"));
            Log.Logger.Debug("Getting ASPNETCORE_ENVIRONMENT: {ASPNETCORE_ENVIRONMENT}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            // Implement dependency injection in ~/Program.Main()
            var host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
                {
                    // This method gets called by the runtime. Use this method to add services to the container.
                    services.AddSingleton<IExampleService, StartupService>();
                }).UseSerilog().Build();

            //host.RunAsync();

            // Use dependency injection in ~/Program.Main()
            var service = ActivatorUtilities.CreateInstance<StartupService>(host.Services);

            service.GetStartupMessage();

            Log.Logger.Information("Stopping ...");

            Log.CloseAndFlush();
        }
    }
}
