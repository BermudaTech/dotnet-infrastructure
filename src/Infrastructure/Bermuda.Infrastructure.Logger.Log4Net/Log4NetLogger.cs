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
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Init(string configPath = null)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(string.IsNullOrEmpty(configPath) ? "log4net.config" : configPath));
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }

        public void Write(
            LogType logType,
            string message)
        {
            this.LogWrite(logType, message, null);
        }

        public void Write(
            LogType logType,
            string message,
            Exception ex)
        {
            this.LogWrite(logType, message, ex);
        }

        public void Write(
            LogType logType,
            string message,
            params string[] parameters)
        {
            this.LogWrite(logType, message, null, parameters);
        }

        public void Write(
            LogType logType,
            string message,
            Exception ex,
            params string[] parameters)
        {
            this.LogWrite(logType, message, ex, parameters);
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
                        Log.Error(logMessage, ex);
                    else
                        Log.Error(logMessage);
                    break;
                case LogType.Warning:
                    if (ex != null)
                        Log.Warn(logMessage, ex);
                    else
                        Log.Warn(logMessage);
                    break;
                case LogType.Info:
                    if (ex != null)
                        Log.Info(logMessage, ex);
                    else
                        Log.Info(logMessage);
                    break;
                case LogType.Debug:
                    if (ex != null)
                        Log.Debug(logMessage, ex);
                    else
                        Log.Debug(logMessage);
                    break;
            }
        }
    }
}
