﻿using Microsoft.Extensions.Hosting;
using Serilog;
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
    LogEventLevel minimumLevel = LogEventLevel.Information)
    {
        ArgumentNullException.ThrowIfNull(hostBuilder, nameof(hostBuilder));
        ArgumentNullException.ThrowIfNull(applicationName, nameof(applicationName));
        ArgumentNullException.ThrowIfNull(environment, nameof(environment));
        ArgumentNullException.ThrowIfNull(newRelicLicenseKey, nameof(newRelicLicenseKey));

        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Is(minimumLevel)
        .Enrich.WithProperty("tenant", tenant)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironment(environment)
        .WriteTo.NewRelicLogs(licenseKey: newRelicLicenseKey, applicationName: applicationName, restrictedToMinimumLevel: minimumLevel)
        .WriteTo.Console()
        .CreateLogger();

        Log.Information("Serilog + NewRelic logger started. App: {Application}, Env: {Environment}, Tenant: {Tenant}, LogLevel: {LogLevel}", applicationName, environment, tenant, minimumLevel);

        return hostBuilder.UseSerilog();
    }
}
