using Bermuda.Core.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace Bermuda.Infrastructure.Logger.Log4Net;

public static class Extension
{
    public static IServiceCollection AddLog4NetLogger(this IServiceCollection services, string configPath = null)
    {
        Log4NetLogger.Init(configPath);
        services.AddSingleton<ILogger, Log4NetLogger>();
        services.AddSingleton(typeof(ILogger<>), typeof(Log4NetLogger<>));
        return services;
    }
}
