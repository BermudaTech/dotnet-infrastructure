using System;

namespace Bermuda.Core.Logger;

public interface ILogger
{
    void Write(LogType logType, string message);
    void Write(LogType logType, string message, Exception ex);
    void Write(LogType logType, string message, params string[] parameters);
    void Write(LogType logType, string message, Exception ex, params string[] parameters);
    IDisposable GenerateCorrelationId(string correlationId = null);
}