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
        string newRelicLicenseKey)
        {
            ArgumentNullException.ThrowIfNull(hostBuilder, nameof(hostBuilder));
            ArgumentNullException.ThrowIfNull(applicationName, nameof(applicationName));
            ArgumentNullException.ThrowIfNull(environment, nameof(environment));
            ArgumentNullException.ThrowIfNull(newRelicLicenseKey, nameof(newRelicLicenseKey));

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("Application", applicationName)
                .Enrich.WithProperty("Tenant", tenant)
                .Enrich.WithProperty("Environment", environment)
                .Enrich.FromLogContext()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.NewRelicLogs(newRelicLicenseKey, applicationName)
                .WriteTo.Console()
                .CreateLogger();

            return hostBuilder.UseSerilog();
        }
    }
}
