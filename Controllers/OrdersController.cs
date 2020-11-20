using Microsoft.AspNetCore.Mvc;
using OrdersReportApp.Models.Order;
using System.Threading.Tasks;

namespace OrdersReportApp.Controllers
{
    public class OrdersController : Controller
    {
        protected IOrderDataAccess OrderDataAccess { get; }
        public OrdersController(IOrderDataAccess orderDataAccess)
        {
            OrderDataAccess = orderDataAccess;
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

    }
}
