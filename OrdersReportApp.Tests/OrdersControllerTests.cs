using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using OrdersReportApp.Controllers;
using OrdersReportApp.Models;
using OrdersReportApp.Models.Order;
using OrdersReportApp.Models.Order.Models;
using OrdersReportApp.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using Xunit;

namespace OrdersReportApp.Tests
{
    // TODO: Implement test cases for exceptions cases
    public class OrdersControllerTests : BaseTester
    {
        public OrderController Controller { get; set; }
        public OrdersControllerTests()
            : base()
        {
            Controller = Container.Resolve<OrderController>();
        }

        [Fact]
        public void IndexSuccessTestCase()
        {
            var result = Controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task GetOrdersSuccessTestCase()
        {
            var result = await Controller.GetOrders();

            var viewResult = Assert.IsType<JsonResult>(result);
            var orderTable = Assert.IsAssignableFrom<OrdersTable>(viewResult.Value);
            Assert.Equal(GetCustomOrders().Count(), orderTable.Data.Count());
        }

        [Fact]
        public async Task AddOrderSuccessTestCase()
        {
            var mockDbContext = Container.Resolve<Mock<DatabaseContext>>();
            var newOrderViewModel = new NewOrderViewModel()
            {
                Date = DateTime.Now,
                Price = 123.45m
            };

            var result = await Controller.AddOrder(newOrderViewModel);

            var viewResult = Assert.IsType<JsonResult>(result);
            Assert.IsAssignableFrom<RequestState>(viewResult.Value);
            mockDbContext.Verify(context => context.AddAsync(It.IsAny<Order>(), CancellationToken.None));
            mockDbContext.Verify(context => context.SaveChangesAsync(CancellationToken.None));
        }

        [Fact]
        public async Task UpdateOrderSuccessTestCase()
        {
            var mockDbSet = Container.Resolve<Mock<DbSet<Order>>>();
            var mockDbContext = Container.Resolve<Mock<DatabaseContext>>();
            var updatingOrderViewModel = new UpdatingOrderViewModel()
            {
                Id = new Guid("54bc5e85-e12f-455a-892c-0122638934bc"),
                Date = DateTime.Now,
                Price = 123.45m
            };

            var result = await Controller.UpdateOrder(updatingOrderViewModel);

            var viewResult = Assert.IsType<JsonResult>(result);
            Assert.IsAssignableFrom<RequestState>(viewResult.Value);
            mockDbSet.Verify(context => context.Update(It.IsAny<Order>()));
            mockDbContext.Verify(context => context.SaveChangesAsync(CancellationToken.None));
        }

        [Fact]
        public async Task RemoveOrderSuccessTestCase()
        {
            var mockDbContext = Container.Resolve<Mock<DatabaseContext>>();
            var deletingOrder = new Order()
            {
                Id = new Guid("54bc5e85-e12f-455a-892c-0122638934bc"),
                Date = DateTime.Parse("2011-02-11"),
                Price = 123.43m
            };

            var result = await Controller.RemoveOrder(deletingOrder);

            var viewResult = Assert.IsType<JsonResult>(result);
            Assert.IsAssignableFrom<RequestState>(viewResult.Value);
            mockDbContext.Verify(context => context.Remove(deletingOrder));
            mockDbContext.Verify(context => context.SaveChangesAsync(CancellationToken.None));
        }

        [Fact]
        public async void UploadReportSuccessTestCase()
        {
            ReportViewModel reportViewModel = new ReportViewModel();
            reportViewModel.From = new DateTime(1968, 3, 12);
            reportViewModel.To = new DateTime(2020, 11, 21);

            var result = await Controller.UploadReport(reportViewModel);

            var fileStreamResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("Report.xlsx", fileStreamResult.FileDownloadName);

            // TODO: Check generated file content on future
        }
    }
}
