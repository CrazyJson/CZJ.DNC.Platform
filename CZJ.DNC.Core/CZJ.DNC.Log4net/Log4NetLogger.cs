using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace CZJ.DNC.Log4net
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        private static ILoggerRepository repository { get; set; }

        private static void InitLogConfig()
        {
            string baseDir = AppContext.BaseDirectory;
            FileInfo fi = new FileInfo(AppContext.BaseDirectory + "log4net.config");
            XmlDocument doc = new XmlDocument();
            using (StreamReader sr = fi.OpenText())
            {
                doc.Load(sr);
                XmlNodeList list = doc.SelectNodes("//appender/file");
                foreach (XmlNode node in list)
                {
                    var attr = node.Attributes["value"];
                    if (attr == null)
                    {
                        continue;
                    }
                    attr.InnerText = baseDir + (attr.InnerText.StartsWith("\\") ? attr.InnerText.Substring(1) : attr.InnerText);
                }
            }

            repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            XmlConfigurator.Configure(repository, doc["log4net"]);
        }

        public Log4NetLogger(string name)
        {
            if (repository == null)
            {
                InitLogConfig();
            }
            _log = LogManager.GetLogger(repository.Name, name);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            string message = null;
            if (null != formatter)
            {
                message = formatter(state, exception);
            }
            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        _log.Fatal(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        _log.Debug(message);
                        break;
                    case LogLevel.Error:
                        _log.Error(message, exception);
                        break;
                    case LogLevel.Information:
                        _log.Info(message);
                        break;
                    case LogLevel.Warning:
                        _log.Warn(message);
                        break;
                    default:
                        _log.Warn($"遇到未知日志级别{logLevel}");
                        _log.Info(message, exception);
                        break;
                }
            }
        }
    }
}

