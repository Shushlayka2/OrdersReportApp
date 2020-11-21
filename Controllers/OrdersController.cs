﻿using Microsoft.AspNetCore.Mvc;
using OrdersReportApp.Models.Order;
using OrdersReportApp.Services;
using OrdersReportApp.ViewModels;
using System.Threading.Tasks;

namespace OrdersReportApp.Controllers
{
    // TODO: Handle exception cases
    public class OrdersController : Controller
    {
        protected IOrderDataAccess OrderDataAccess { get; }
        protected IOrdersReporter OrdersReporter { get; }

        public OrdersController(IOrderDataAccess orderDataAccess, IOrdersReporter ordersReporter)
        {
            OrderDataAccess = orderDataAccess;
            OrdersReporter = ordersReporter;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(OrderDataAccess.GetOrders());
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            await OrderDataAccess.AddNewOrderAsync(order);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            await OrderDataAccess.UpdateOrderAsync(order);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOrder(Order order)
        {
            await OrderDataAccess.RemoveOrderAsync(order);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UploadReport(ReportViewModel reportViewModel)
        {
            await OrdersReporter.CreateReportAsync(reportViewModel);
            return Ok();
        }
    }
}
