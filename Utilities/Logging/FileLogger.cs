using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace OrdersReportApp.Utilities.Logging
{
    public class FileLogger : ILogger
    {
        private string filePath;
        private bool isEnable = true;
        private static object _lock = new object();

        public FileLogger(IConfiguration configuration)
        {
            var logFileSection = configuration.GetSection("Logging").GetSection("File");
            if (logFileSection != null)
            {
                filePath = logFileSection.GetSection("LoggingFiles:Path").Value;
            }
            else
                isEnable = false;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return isEnable;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
