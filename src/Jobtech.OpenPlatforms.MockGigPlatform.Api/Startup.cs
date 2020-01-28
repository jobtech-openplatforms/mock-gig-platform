using Jobtech.OpenPlatforms.MockGigPlatform.Api.Config;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
              .CreateLogger();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // App Settings
            services.Configure<RavenConfig>(Configuration.GetSection("Raven"));
            services.Configure<GigDataServiceConfig>(Configuration.GetSection("GigDataService"));

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            //Add functionality to inject IOptions<T> 
            services.AddOptions();

            services.AddLogging(configure => {
                configure.AddConsole();
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
                loggingBuilder.AddApplicationInsights("2bf8e095-407c-4883-9f04-40a38c2568a7");
            });
            services.AddApplicationInsightsTelemetry("2bf8e095-407c-4883-9f04-40a38c2568a7");

            // Document store for Raven
            services.AddSingleton<IDocumentStoreHolder, DocumentStoreHolder>();


            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, RavenDatabaseCheckService>();
            services.AddTransient<Microsoft.Extensions.Hosting.IHostedService, RavenDatabaseCheckService>(); // Run at startup

            services.AddHttpClient<IMockPlatformHttpClient, MockPlatformHttpClient>();

            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseMvc();
        }
    }
}
