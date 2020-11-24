using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrdersReportApp.Models;
using OrdersReportApp.Models.Order;
using OrdersReportApp.Models.Order.Models;
using OrdersReportApp.Services;
using OrdersReportApp.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OrdersReportApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> Logger;
        private readonly IMapper Mapper;

        protected IOrderDataAccess OrderDataAccess { get; }
        protected IOrdersReporter OrdersReporter { get; }

        public OrderController(
            IMapper mapper,
            IOrderDataAccess orderDataAccess,
            IOrdersReporter ordersReporter,
            ILogger<OrderController> logger)
        {
            Logger = logger;
            Mapper = mapper;
            OrderDataAccess = orderDataAccess;
            OrdersReporter = ordersReporter;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await OrderDataAccess.GetOrdersAsync();
                return Json(new OrdersTable(orders));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return Json(new { data = "" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(NewOrderViewModel newOrder)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new RequestState(Status.Fail, "Ошибка валидации", ModelState));

                var order = Mapper.Map<Order>(newOrder);
                await OrderDataAccess.AddOrderAsync(order);

                return Json(new RequestState(Status.Success, "Заказ успешно добавлен"));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return Json(new { status = false, message = "Не удалось выполнить требуемую операцию." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdatingOrderViewModel updatingOrder)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new RequestState(Status.Fail, "Ошибка валидации", ModelState));

                var order = Mapper.Map<Order>(updatingOrder);
                await OrderDataAccess.UpdateOrderAsync(order);

                return Json(new RequestState(Status.Success, "Заказ успешно изменен"));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return Json(new RequestState(Status.Fail, "Не удалось выполнить требуемую операцию"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOrder(Order order)
        {
            try
            {
                await OrderDataAccess.RemoveOrderAsync(order);

                return Json(new RequestState(Status.Success, "Заказ успешно удален"));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return Json(new RequestState(Status.Fail, "Не удалось выполнить требуемую операцию"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadReport(ReportViewModel reportViewModel)
        {
            try
            {
                var path = await OrdersReporter.CreateReportAsync(reportViewModel);
                var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);

                return File(
                    fileStream: fs,
                    contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileDownloadName: "Report.xlsx");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
