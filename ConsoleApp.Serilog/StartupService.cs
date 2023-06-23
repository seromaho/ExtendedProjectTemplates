using Microsoft.Extensions.Configuration;
// Implementation of key-value pair based configuration for Microsoft.Extensions.Configuration.
// Includes the memory configuration provider.
using Microsoft.Extensions.Logging;
// Logging infrastructure default implementation for Microsoft.Extensions.Logging.

namespace ConsoleApp.Serilog
{
    internal class StartupService : IExampleService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StartupService> _logger;

        /// <summary>
        /// Instance constructor
        /// </summary>
        /// <param name="configuration"></param>
        public StartupService(IConfiguration configuration, ILogger<StartupService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Gets the startup message specified in the app settings, and writes it to the standard output stream.
        /// </summary>
        public void GetStartupMessage()
        {
            // Use the { ILogger } instance from the dependency injection container
            _logger.LogInformation("Getting startup message from configuration: {startupMessage}",
                _configuration.GetValue<string>("StartupService:StartupMessage", "Hello World!"));

            // Get values from the configuration object given their key and their target type
            Console.WriteLine(_configuration
                .GetSection("StartupService")
                .GetValue<string>("StartupMessage", "Hello World!"));
            // Alternative syntax:
            // Console.WriteLine(_configuration.GetValue<string>("StartupService:StartupMessage", "Hello World!"));
        }
    }
}
