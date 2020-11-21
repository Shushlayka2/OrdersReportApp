using Microsoft.Extensions.Configuration;
using OrdersReportApp.Controllers;
using OrdersReportApp.Models.Order;
using OrdersReportApp.Services;
using Unity;

namespace OrdersReportApp.Tests
{
    public class BaseTester
    {
        protected IUnityContainer Container { get; } = new UnityContainer();
        public BaseTester(string jsonFilePath = "appsettings.json")
        {
            var builder = new ConfigurationBuilder().AddJsonFile(jsonFilePath);
            Container.RegisterInstance<IConfiguration>(builder.Build());
            Container.RegisterType<IOrdersReporter, OrdersReporter>();
            Container.RegisterType<IOrderDataAccess, OrderDataAccess>();
            Container.RegisterType<OrdersController>();
        }
    }
}
