using System;

namespace Bermuda.Core.Logger;

public interface ILogger
{
    void Write(LogType logType, string message);
    void Write(LogType logType, string message, Exception ex);
    void Write(LogType logType, string message, params object[] parameters);
    void Write(LogType logType, string message, Exception ex, params object[] parameters);
    IDisposable GenerateCorrelationId(string correlationId = null);
}

public interface ILogger<T>
{
    void Write(LogType logType, string message);
    void Write(LogType logType, string message, Exception ex);
    void Write(LogType logType, string message, params object[] parameters);
    void Write(LogType logType, string message, Exception ex, params object[] parameters);
    IDisposable GenerateCorrelationId(string correlationId = null);
}