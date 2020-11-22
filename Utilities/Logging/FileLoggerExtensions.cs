using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OrdersReportApp.Utilities.Logging
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, IConfiguration configuration)
        {
            factory.AddProvider(new FileLoggerProvider(configuration));
            return factory;
        }
    }
}
