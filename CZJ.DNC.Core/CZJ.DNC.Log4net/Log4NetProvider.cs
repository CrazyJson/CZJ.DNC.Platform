using CZJ.Dependency;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CZJ.DNC.Log4net
{
    public class Log4NetProvider : ILoggerProvider, ISingletonDependency
    {
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers =
         new ConcurrentDictionary<string, Log4NetLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(name);
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
