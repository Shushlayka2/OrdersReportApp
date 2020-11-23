using OrdersReportApp.Controllers;
using OrdersReportApp.ViewModels;
using System;
using Unity;
using Xunit;

namespace OrdersReportApp.Tests
{
    // TODO: Add assert sections to tests for excluding part of selfhand testing 
    public class OrdersControllerTests : BaseTester
    {
        public OrderController Controller { get; set; }
        public OrdersControllerTests()
            : base()
        {
            Controller = Container.Resolve<OrderController>();
        }

        [Fact]
        public async void UploadReportWithFilter()
        {
            //arrange
            ReportViewModel reportViewModel = new ReportViewModel();
            reportViewModel.From = new DateTime(1968, 3, 12);
            reportViewModel.To = new DateTime(2020, 11, 21);
            
            //act
            await Controller.UploadReport(reportViewModel);
        }

        [Fact]
        public async void UploadReportWithoutFilter()
        {
            //arrange
            ReportViewModel reportViewModel = new ReportViewModel();

            //act
            await Controller.UploadReport(reportViewModel);
        }
    }
}
