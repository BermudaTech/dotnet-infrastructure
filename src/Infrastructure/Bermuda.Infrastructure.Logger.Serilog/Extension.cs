using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;


namespace Bermuda.Infrastructure.Logger.Serilog
{
    public static class Extension
    {
        public static IHostBuilder UseSerilogWithNewRelic(
        this IHostBuilder hostBuilder,
        string applicationName,
        string tenant,
        string environment,
        string newRelicLicenseKey,
        LogEventLevel minimumLevel = LogEventLevel.Information)
        {
            ArgumentNullException.ThrowIfNull(hostBuilder, nameof(hostBuilder));
            ArgumentNullException.ThrowIfNull(applicationName, nameof(applicationName));
            ArgumentNullException.ThrowIfNull(environment, nameof(environment));
            ArgumentNullException.ThrowIfNull(newRelicLicenseKey, nameof(newRelicLicenseKey));

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(minimumLevel)
            .Enrich.WithProperty("Application", applicationName)
            .Enrich.WithProperty("Tenant", tenant)
            .Enrich.WithProperty("Environment", environment)
            .Enrich.FromLogContext()
            .MinimumLevel.Override("Microsoft", minimumLevel)
            .WriteTo.NewRelicLogs(licenseKey: newRelicLicenseKey, applicationName: applicationName)
            .WriteTo.Console()
            .CreateLogger();

            Log.Information("Serilog + NewRelic logger started. App: {Application}, Env: {Environment}, Tenant: {Tenant}", applicationName, environment, tenant);

            return hostBuilder.UseSerilog();
        }
    }
}
