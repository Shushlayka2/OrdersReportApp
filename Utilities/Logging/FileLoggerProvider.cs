using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OrdersReportApp.Utilities.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private IConfiguration configuration;
        public FileLoggerProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(configuration);
        }

        public void Dispose()
        {
        }
    }
}
