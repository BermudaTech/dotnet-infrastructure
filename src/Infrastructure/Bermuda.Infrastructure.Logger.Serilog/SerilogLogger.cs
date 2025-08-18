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
            }
            catch (FormatException e)
            {
                messageToLog = templateOrMessage + " | Parameters: " + string.Join(", ", parameters);
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
                if (ex != null) Log.Error(ex, messageToLog);
                else Log.Error(messageToLog);
                break;

            case LogType.Warning:
                if (ex != null) Log.Warning(ex, messageToLog);
                else Log.Warning(messageToLog);
                break;

            case LogType.Info:
                if (ex != null) Log.Information(ex, messageToLog);
                else Log.Information(messageToLog);
                break;

            case LogType.Debug:
                if (ex != null) Log.Debug(ex, messageToLog);
                else Log.Debug(messageToLog);
                break;

            case LogType.Verbose:
                if (ex != null) Log.Verbose(ex, messageToLog);
                else Log.Verbose(messageToLog);
                break;

            case LogType.Fatal:
                if (ex != null) Log.Fatal(ex, messageToLog);
                else Log.Fatal(messageToLog);
                break;

            default:
                if (ex != null) Log.Information(ex, messageToLog);
                else Log.Information(messageToLog);
                break;
        }
    }
}