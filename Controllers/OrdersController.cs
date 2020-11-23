using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrdersReportApp.Models.Order;
using OrdersReportApp.Services;
using OrdersReportApp.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace OrdersReportApp.Controllers
{
    // TODO: Handle exception cases
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> Logger;
        protected IOrderDataAccess OrderDataAccess { get; }
        protected IOrdersReporter OrdersReporter { get; }

        public OrdersController(
            IOrderDataAccess orderDataAccess,
            IOrdersReporter ordersReporter,
            ILogger<OrdersController> logger)
        {
            Logger = logger;
            OrderDataAccess = orderDataAccess;
            OrdersReporter = ordersReporter;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await OrderDataAccess.GetOrdersAsync();
            return Json(new { data = orders});
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(NewOrderViewModel newOrder)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { status = false, message = "Ошибка валидации", model_state = ModelState });

                await OrderDataAccess.AddNewOrderAsync(newOrder);
                return Json(new { status = true, message = "Заказ успешно добавлен." });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return Json(new { success = false, message = "Не удалось выполнить требуемую операцию." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { status = false, message = "Ошибка валидации", model_state = ModelState });

                await OrderDataAccess.UpdateOrderAsync(order);
                return Json(new { status = true, message = "Заказ успешно изменен." });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return Json(new { success = false, message = "Не удалось выполнить требуемую операцию." });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOrder(Order order)
        {
            try
            {
                await OrderDataAccess.RemoveOrderAsync(order);
                return Json(new { status = true, message = "Заказ успешно удален." });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return Json(new { success = false, message = "Не удалось выполнить требуемую операцию." });
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
                return RedirectToAction("Error");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
