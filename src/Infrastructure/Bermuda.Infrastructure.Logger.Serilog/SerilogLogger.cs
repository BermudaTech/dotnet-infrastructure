using Bermuda.Core.Logger;
using Serilog;
using Serilog.Context;

namespace Bermuda.Infrastructure.Logger.Serilog;

public class SerilogLogger : Core.Logger.ILogger
{
    public IDisposable GenerateCorrelationId(string correlationId = null)
    {
        var id = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid().ToString() : correlationId;
        return LogContext.PushProperty("CorrelationId", id);
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
                parameters = Array.Empty<string>();
            }
            catch (FormatException e)
            {
                messageToLog = templateOrMessage + " | Parameters: " + string.Join(", ", parameters);
                parameters = Array.Empty<string>();
                Log.Error(e, "SERILOG: Failed to format log message: {Message}", templateOrMessage);
            }
        }
        else
        {
            messageToLog = templateOrMessage;
        }

        switch (logType)
        {
            case LogType.Error:
                if (ex != null) Log.Error(ex, messageToLog, parameters);
                else Log.Error(messageToLog, parameters);
                break;

            case LogType.Warning:
                if (ex != null) Log.Warning(ex, messageToLog, parameters);
                else Log.Warning(messageToLog, parameters);
                break;

            case LogType.Info:
                if (ex != null) Log.Information(ex, messageToLog, parameters);
                else Log.Information(messageToLog, parameters);
                break;

            case LogType.Debug:
                if (ex != null) Log.Debug(ex, messageToLog, parameters);
                else Log.Debug(messageToLog, parameters);
                break;

            case LogType.Verbose:
                if (ex != null) Log.Verbose(ex, messageToLog, parameters);
                else Log.Verbose(messageToLog, parameters);
                break;

            case LogType.Fatal:
                if (ex != null) Log.Fatal(ex, messageToLog, parameters);
                else Log.Fatal(messageToLog, parameters);
                break;

            default:
                if (ex != null) Log.Information(ex, messageToLog, parameters);
                else Log.Information(messageToLog, parameters);
                break;
        }
    }
}