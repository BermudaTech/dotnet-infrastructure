using Bermuda.Core.Logger;
using Microsoft.Extensions.Logging;

namespace Bermuda.Infrastructure.Logger.Serilog;

public class SerilogLogger : Core.Logger.ILogger
{
    private readonly Microsoft.Extensions.Logging.ILogger<SerilogLogger> logger;

    public SerilogLogger(Microsoft.Extensions.Logging.ILogger<SerilogLogger> logger)
    {
        this.logger = logger;
    }

    public IDisposable GenerateCorrelationId(string correlationId = null)
    {
        var id = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid().ToString() : correlationId;

        // Microsoft logger’da CorrelationId taşımak için scope kullanılır.
        return logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = id
        });
    }

    public void Write(LogType logType, string message)
    {
        LogWrite(logType, message, null);
    }

    public void Write(LogType logType, string message, Exception ex)
    {
        LogWrite(logType, message, ex);
    }

    public void Write(LogType logType, string message, params object[] parameters)
    {
        LogWrite(logType, message, null, parameters);
    }

    public void Write(LogType logType, string message, Exception ex, params object[] parameters)
    {
        LogWrite(logType, message, ex, parameters);
    }

    private void LogWrite(LogType logType, string message, Exception ex = null, params object[] parameters)
    {

        switch (logType)
        {
            case LogType.Error:
                if (ex != null) logger.LogError(ex, message, parameters);
                else logger.LogError(message, parameters);
                break;

            case LogType.Warning:
                if (ex != null) logger.LogWarning(ex, message, parameters);
                else logger.LogWarning(message, parameters);
                break;

            case LogType.Info:
                if (ex != null) logger.LogInformation(ex, message, parameters);
                else logger.LogInformation(message, parameters);
                break;

            case LogType.Debug:
                if (ex != null) logger.LogDebug(ex, message, parameters);
                else logger.LogDebug(message, parameters);
                break;

            case LogType.Verbose:
                if (ex != null) logger.LogTrace(ex, message, parameters);
                else logger.LogTrace(message, parameters);
                break;

            case LogType.Fatal:
                if (ex != null) logger.LogCritical(ex, message, parameters);
                else logger.LogCritical(message, parameters);
                break;

            default:
                if (ex != null) logger.LogInformation(ex, message, parameters);
                else logger.LogInformation(message, parameters);
                break;
        }
    }
}

public class SerilogLogger<T> : Core.Logger.ILogger<T>
{
    private readonly Microsoft.Extensions.Logging.ILogger<T> logger;

    public SerilogLogger(Microsoft.Extensions.Logging.ILogger<T> logger)
    {
        this.logger = logger;
    }

    public IDisposable GenerateCorrelationId(string correlationId = null)
    {
        var id = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid().ToString() : correlationId;

        // Microsoft logger’da CorrelationId taşımak için scope kullanılır.
        return logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = id
        });
    }

    public void Write(LogType logType, string message)
    {
        LogWrite(logType, message, null);
    }

    public void Write(LogType logType, string message, Exception ex)
    {
        LogWrite(logType, message, ex);
    }

    public void Write(LogType logType, string message, params object[] parameters)
    {
        LogWrite(logType, message, null, parameters);
    }

    public void Write(LogType logType, string message, Exception ex, params object[] parameters)
    {
        LogWrite(logType, message, ex, parameters);
    }

    private void LogWrite(LogType logType, string message, Exception ex = null, params object[] parameters)
    {
        switch (logType)
        {
            case LogType.Error:
                if (ex != null) logger.LogError(ex, message, parameters);
                else logger.LogError(message, parameters);
                break;

            case LogType.Warning:
                if (ex != null) logger.LogWarning(ex, message, parameters);
                else logger.LogWarning(message, parameters);
                break;

            case LogType.Info:
                if (ex != null) logger.LogInformation(ex, message, parameters);
                else logger.LogInformation(message, parameters);
                break;

            case LogType.Debug:
                if (ex != null) logger.LogDebug(ex, message, parameters);
                else logger.LogDebug(message, parameters);
                break;

            case LogType.Verbose:
                if (ex != null) logger.LogTrace(ex, message, parameters);
                else logger.LogTrace(message, parameters);
                break;

            case LogType.Fatal:
                if (ex != null) logger.LogCritical(ex, message, parameters);
                else logger.LogCritical(message, parameters);
                break;

            default:
                if (ex != null) logger.LogInformation(ex, message, parameters);
                else logger.LogInformation(message, parameters);
                break;
        }
    }
}