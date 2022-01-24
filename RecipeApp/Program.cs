using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace RecipeApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            var host = BuildWebHost(configuration, args);
            host.Run();
        }

        public static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .UseStartup<Startup>()
                .Build();

        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

    }
}
