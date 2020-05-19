using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).ConfigureAppConfiguration((hostContext, configApp) =>
            {
                configApp.SetBasePath(Directory.GetCurrentDirectory());
                configApp.AddJsonFile("appsettings.json", false, true);
                configApp.AddJsonFile(
                    $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                    optional: true);
                configApp.AddJsonFile("/app/secrets/appsettings.secrets.json", optional: true);
                configApp.AddJsonFile("appsettings.local.json", optional: true,
                    reloadOnChange: false); //load local settings

                configApp.AddEnvironmentVariables();
            }).Build();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        //public static void Main(string[] args)
        //{
        //    Log.Logger = new LoggerConfiguration()
        //        .MinimumLevel.Debug()
        //        .WriteTo.Console()
        //        .WriteTo.ApplicationInsights(TelemetryConfiguration.Active, TelemetryConverter.Traces)
        //        .CreateLogger();
        //    CreateWebHostBuilder(args).Build().Run();
        //}

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseApplicationInsights()
        //    .ConfigureLogging((hostingContext, logging) =>
        //    {
        //        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        //        logging.AddEventSourceLogger();
        //        logging.AddConsole();
        //    })
        //        .UseStartup<Startup>();
    }
}
