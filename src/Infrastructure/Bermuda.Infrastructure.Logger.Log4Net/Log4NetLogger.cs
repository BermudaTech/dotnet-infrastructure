using Bermuda.Core.Logger;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Bermuda.Infrastructure.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(string loggerName = null)
        {
            _log = string.IsNullOrEmpty(loggerName) ? LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType) : LogManager.GetLogger(loggerName);
        }

        public static void Init(string configPath = null)
        {
            var log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(string.IsNullOrEmpty(configPath) ? "log4net.config" : configPath));
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }

        public void Write(
            LogType logType,
            string message)
        {
            LogWrite(logType, message, null);
        }

        public void Write(
            LogType logType,
            string message,
            Exception ex)
        {
            LogWrite(logType, message, ex);
        }

        public void Write(
            LogType logType,
            string message,
            params string[] parameters)
        {
            LogWrite(logType, message, null, parameters);
        }

        public void Write(
            LogType logType,
            string message,
            Exception ex,
            params string[] parameters)
        {
            LogWrite(logType, message, ex, parameters);
        }

        private void LogWrite(
            LogType logType,
            string message,
            Exception ex,
            params string[] parameters)
        {
            string logMessage = String.Empty;

            if (parameters != null && parameters.Length > 0)
            {
                logMessage = String.Format(message, parameters);
            }
            else
            {
                logMessage = message;
            }

            switch (logType)
            {
                case LogType.Error:
                    if (ex != null)
                        _log.Error(logMessage, ex);
                    else
                        _log.Error(logMessage);
                    break;
                case LogType.Warning:
                    if (ex != null)
                        _log.Warn(logMessage, ex);
                    else
                        _log.Warn(logMessage);
                    break;
                case LogType.Info:
                    if (ex != null)
                        _log.Info(logMessage, ex);
                    else
                        _log.Info(logMessage);
                    break;
                case LogType.Debug:
                    if (ex != null)
                        _log.Debug(logMessage, ex);
                    else
                        _log.Debug(logMessage);
                    break;
            }
        }
    }
}