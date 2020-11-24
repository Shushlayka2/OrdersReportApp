using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using OrdersReportApp.Controllers;
using OrdersReportApp.Models.Order;
using OrdersReportApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace OrdersReportApp.Tests
{
    public class BaseTester
    {
        protected IUnityContainer Container { get; } = new UnityContainer();
        public BaseTester(string jsonFilePath = "appsettings.json")
        {
            var dataSet = GetCustomOrders().AsQueryable();
            var mockSet = new Mock<DbSet<Order>>();
            mockSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(dataSet.Provider);
            mockSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(dataSet.Expression);
            mockSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(dataSet.ElementType);
            mockSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(dataSet.GetEnumerator());


            var mockDBContext = new Mock<DatabaseContext>();
            mockDBContext.Setup(m => m.Orders).Returns(mockSet.Object);

            // TODO: Implement simulation of data changing in future test cases
            //mockDBContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
            //mockDBContext.Setup(m => m.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            //    .Callback((Order order, CancellationToken token) => { data.Add(order); })
            //    .Returns((Order order, CancellationToken token) => new ValueTask<EntityEntry<Order>>());

            var builder = new ConfigurationBuilder().AddJsonFile(jsonFilePath);
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            Container.RegisterInstance(mockSet);
            Container.RegisterInstance(mockDBContext);
            Container.RegisterInstance(mockDBContext.Object);
            Container.RegisterInstance<IConfiguration>(builder.Build());
            Container.RegisterType<IOrdersReporter, OrdersReporter>();
            Container.RegisterType<IOrderDataAccess, OrderDataAccess>();
            Container.RegisterInstance(mapperConfig.CreateMapper());
            Container.RegisterInstance(new Mock<ILogger<OrderController>>().Object);
            Container.RegisterType<OrderController>();
        }

        protected List<Order> GetCustomOrders()
        {
            return new List<Order>
            {
                new Order { Id = new Guid("54bc5e85-e12f-455a-892c-0122638934bc"), Date = DateTime.Parse("2011-02-11"), Price = 123.43m },
                new Order { Id = new Guid("27b038c4-e35b-490c-b5cf-01b919014ef3"), Date = DateTime.Parse("2000-02-12"), Price = 214m },
                new Order { Id = new Guid("8161a0fa-2374-4f10-9680-038faf53682c"), Date = DateTime.Parse("2011-02-13"), Price = 214321.32m },
                new Order { Id = new Guid("aa9dae97-ef76-4309-bcf0-0409dd9edc81"), Date = DateTime.Parse("2011-02-14"), Price = 12343m },
                new Order { Id = new Guid("eeff08b7-d2a8-437e-8ec0-046f26d487e7"), Date = DateTime.Parse("2011-02-15"), Price = 4325.7m },
                new Order { Id = new Guid("165c3823-ca64-4690-8923-0541c3ee174f"), Date = DateTime.Parse("2011-02-16"), Price = 5674.45m },
                new Order { Id = new Guid("2dbfadb7-588d-4c2d-968e-055f91f2df76"), Date = DateTime.Parse("2011-02-17"), Price = 467315m },
                new Order { Id = new Guid("42ab2b30-3ddf-46bb-846a-05dcddccdc05"), Date = DateTime.Parse("2011-02-18"), Price = 65762.1m },
                new Order { Id = new Guid("cadac935-83f9-45b5-8c92-068292f2dc39"), Date = DateTime.Parse("2011-02-19"), Price = 2345.21m },
            };
        }
    }
}
