using Bermuda.Core.Logger;
using Serilog;
using Serilog.Context;


namespace Bermuda.Infrastructure.Logger.Serilog;

public class SerilogLogger : Core.Logger.ILogger
{
    public IDisposable GenerateCorrelationId(string correlationId = null)
    {
        if (string.IsNullOrEmpty(correlationId))
            correlationId = Guid.NewGuid().ToString();

        return LogContext.PushProperty("CorrelationId", correlationId);
    }

    public void Write(LogType logType, string message)
    {
        LogWithSerilog(logType, message, null, null);
    }

    public void Write(LogType logType, string message, Exception ex)
    {
        LogWithSerilog(logType, message, ex, null);
    }

    public void Write(LogType logType, string message, params string[] parameters)
    {
        LogWithSerilog(logType, message, null, parameters);
    }

    public void Write(LogType logType, string message, Exception ex, params string[] parameters)
    {
        LogWithSerilog(logType, message, ex, parameters);
    }

    private void LogWithSerilog(LogType logType, string message, Exception ex, string[] parameters)
    {
        var hasParams = parameters != null && parameters.Length > 0;
        string logTemplate = hasParams
        ? "{Message} | Class: {ClassName} | Method: {MethodName} | Line: {LineNumber} | Args: {@Args}"
        : "{Message}";

        switch (logType)
        {
            case LogType.Error:
                if (ex != null)
                    Log.Error(ex, logTemplate, message, parameters);
                else
                    Log.Error(logTemplate, message, parameters);
                break;
            case LogType.Warning:
                if (ex != null)
                    Log.Warning(ex, logTemplate, message, parameters);
                else
                    Log.Warning(logTemplate, message, parameters);
                break;
            case LogType.Info:
                if (ex != null)
                    Log.Information(ex, logTemplate, message, parameters);
                else
                    Log.Information(logTemplate, message, parameters);
                break;
            case LogType.Debug:
                if (ex != null)
                    Log.Debug(ex, logTemplate, message, parameters);
                else
                    Log.Debug(logTemplate, message, parameters);
                break;
            case LogType.Verbose:
                if (ex != null)
                    Log.Verbose(ex, logTemplate, message, parameters);
                else
                    Log.Verbose(logTemplate, message, parameters);
                break;
            case LogType.Fatal:
                if (ex != null)
                    Log.Fatal(ex, logTemplate, message, parameters);
                else
                    Log.Fatal(logTemplate, message, parameters);
                break;
            default:
                if (ex != null)
                    Log.Information(ex, logTemplate, message, parameters);
                else
                    Log.Information(logTemplate, message, parameters);
                break;
        }
    }
}