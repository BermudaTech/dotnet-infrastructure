using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewRelic.LogEnrichers.Serilog;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;


namespace Bermuda.Infrastructure.Logger.Serilog;

public static class Extension
{
    public static IHostBuilder UseSerilogWithNewRelic(
    this IHostBuilder hostBuilder,
    string applicationName,
    string tenant,
    string environment,
    string newRelicLicenseKey,
    LogLevel minimumLevel = LogLevel.Information)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder, nameof(hostBuilder));
        ArgumentNullException.ThrowIfNull(applicationName, nameof(applicationName));
        ArgumentNullException.ThrowIfNull(environment, nameof(environment));
        ArgumentNullException.ThrowIfNull(newRelicLicenseKey, nameof(newRelicLicenseKey));
        var logEventlevel = ConvertToLogEventLevel(minimumLevel);
        
        //Optional
        SelfLog.Enable(Console.Error.WriteLine);

        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Is(logEventlevel)
        .Enrich.WithProperty("application", applicationName)
        .Enrich.WithProperty("environment", environment)
        .Enrich.WithProperty("tenant", tenant)
        .Enrich.WithProperty("service.name", applicationName)
        .Enrich.WithProperty("container.name", Environment.GetEnvironmentVariable("HOSTNAME"))
        .Enrich.WithProperty("ecs.task.id", Environment.GetEnvironmentVariable("ECS_TASK_ID"))
        .Enrich.FromLogContext()
        .Enrich.WithEnvironment(environment)
        .Enrich.WithNewRelicLogsInContext()
        .WriteTo.Console(restrictedToMinimumLevel: logEventlevel)
        .WriteTo.NewRelicLogs(licenseKey: newRelicLicenseKey, applicationName: applicationName, restrictedToMinimumLevel: logEventlevel)
        .CreateLogger();

        Log.Information("Serilog + NewRelic logger started. App: {Application}, Env: {Environment}, Tenant: {Tenant}, LogLevel: {LogLevel}", applicationName, environment, tenant, logEventlevel);
        
        Environment.SetEnvironmentVariable("NEW_RELIC_LICENSE_KEY", newRelicLicenseKey, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("NEW_RELIC_APP_NAME", applicationName, EnvironmentVariableTarget.Process);

        return hostBuilder.UseSerilog(Log.Logger, dispose: true);
    }

    private static LogEventLevel ConvertToLogEventLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => LogEventLevel.Verbose,
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Critical => LogEventLevel.Fatal,
            LogLevel.None => throw new InvalidOperationException("LogLevel.None cannot be mapped to LogEventLevel."),
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }
}
