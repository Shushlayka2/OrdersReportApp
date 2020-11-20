using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrdersReportApp.Models.Order
{
    public interface IOrderDataAccess
    {
        Task AddNewOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task RemoveOrderAsync(Order order);
        List<Order> GetOrders();
    }
}
