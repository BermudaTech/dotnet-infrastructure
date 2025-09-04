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

    public void Write(LogType logType, string message, params string[] parameters)
    {
        LogWrite(logType, message, null, parameters);
    }

    public void Write(LogType logType, string message, Exception ex, params string[] parameters)
    {
        LogWrite(logType, message, ex, parameters);
    }

    private void LogWrite(LogType logType, string templateOrMessage, Exception ex = null, params string[] parameters)
    {
        string messageToLog;

        if (parameters != null && parameters.Length > 0)
        {
            try
            {
                messageToLog = string.Format(templateOrMessage, parameters);
            }
            catch (FormatException e)
            {
                messageToLog = templateOrMessage + " | Parameters: " + string.Join(", ", parameters);
                logger.LogError(e, "Failed to format log message: {Message}", templateOrMessage);

            }
        }
        else
        {
            messageToLog = templateOrMessage;
        }

        switch (logType)
        {
            case LogType.Error:
                if (ex != null) logger.LogError(ex, messageToLog);
                else logger.LogError(messageToLog);
                break;

            case LogType.Warning:
                if (ex != null) logger.LogWarning(ex, messageToLog);
                else logger.LogWarning(messageToLog);
                break;

            case LogType.Info:
                if (ex != null) logger.LogInformation(ex, messageToLog);
                else logger.LogInformation(messageToLog);
                break;

            case LogType.Debug:
                if (ex != null) logger.LogDebug(ex, messageToLog);
                else logger.LogDebug(messageToLog);
                break;

            case LogType.Verbose:
                if (ex != null) logger.LogTrace(ex, messageToLog);
                else logger.LogTrace(messageToLog);
                break;

            case LogType.Fatal:
                if (ex != null) logger.LogCritical(ex, messageToLog);
                else logger.LogCritical(messageToLog);
                break;

            default:
                if (ex != null) logger.LogInformation(ex, messageToLog);
                else logger.LogInformation(messageToLog);
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

    public void Write(LogType logType, string message, params string[] parameters)
    {
        LogWrite(logType, message, null, parameters);
    }

    public void Write(LogType logType, string message, Exception ex, params string[] parameters)
    {
        LogWrite(logType, message, ex, parameters);
    }

    private void LogWrite(LogType logType, string templateOrMessage, Exception ex = null, params string[] parameters)
    {
        string messageToLog;

        if (parameters != null && parameters.Length > 0)
        {
            try
            {
                messageToLog = string.Format(templateOrMessage, parameters);
            }
            catch (FormatException e)
            {
                messageToLog = templateOrMessage + " | Parameters: " + string.Join(", ", parameters);
                logger.LogError(e, "Failed to format log message: {Message}", templateOrMessage);

            }
        }
        else
        {
            messageToLog = templateOrMessage;
        }

        switch (logType)
        {
            case LogType.Error:
                if (ex != null) logger.LogError(ex, messageToLog);
                else logger.LogError(messageToLog);
                break;

            case LogType.Warning:
                if (ex != null) logger.LogWarning(ex, messageToLog);
                else logger.LogWarning(messageToLog);
                break;

            case LogType.Info:
                if (ex != null) logger.LogInformation(ex, messageToLog);
                else logger.LogInformation(messageToLog);
                break;

            case LogType.Debug:
                if (ex != null) logger.LogDebug(ex, messageToLog);
                else logger.LogDebug(messageToLog);
                break;

            case LogType.Verbose:
                if (ex != null) logger.LogTrace(ex, messageToLog);
                else logger.LogTrace(messageToLog);
                break;

            case LogType.Fatal:
                if (ex != null) logger.LogCritical(ex, messageToLog);
                else logger.LogCritical(messageToLog);
                break;

            default:
                if (ex != null) logger.LogInformation(ex, messageToLog);
                else logger.LogInformation(messageToLog);
                break;
        }
    }
}