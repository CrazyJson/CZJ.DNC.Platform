using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Xml;

namespace CZJ.DNC.Log4net
{
    /// <summary>
    /// 
    /// </summary>
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, Log4NetLogger> loggers =
         new ConcurrentDictionary<string, Log4NetLogger>();

        private const string DefaultLog4NetFileName = "log4net.config";

        private ILoggerRepository loggerRepository { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Log4NetProvider() : this(DefaultLog4NetFileName)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log4NetConfigFileName"></param>
        public Log4NetProvider(string log4NetConfigFileName)
        {
            loggerRepository = LogManager.CreateRepository(
                    Assembly.GetExecutingAssembly(),
                    typeof(log4net.Repository.Hierarchy.Hierarchy));

            if (!Path.IsPathRooted(log4NetConfigFileName))
            {
                log4NetConfigFileName = Path.Combine(AppContext.BaseDirectory, log4NetConfigFileName);
            }
            log4NetConfigFileName = Path.GetFullPath(log4NetConfigFileName);
            var configXml = ParseLog4NetConfigFile(log4NetConfigFileName);
            XmlConfigurator.Configure(this.loggerRepository, configXml.DocumentElement);
        }

        /// <summary>
        /// 创建 logger.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <returns>An instance of the <see cref="ILogger"/>.</returns>
        public ILogger CreateLogger(string categoryName)
            => loggers.GetOrAdd(categoryName, CreateLoggerImplementation);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                return;
            }
            loggers.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(name, loggerRepository.Name);
        }

        /// <summary>
        /// Parses log4net config file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The <see cref="XmlElement"/> with the log4net XML element.</returns>
        private XmlDocument ParseLog4NetConfigFile(string filename)
        {
            using (FileStream fp = File.OpenRead(filename))
            {
                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Prohibit
                };

                var log4netConfig = new XmlDocument();
                using (var reader = XmlReader.Create(fp, settings))
                {
                    log4netConfig.Load(reader);
                }

                return log4netConfig;
            }
        }

    }
}
