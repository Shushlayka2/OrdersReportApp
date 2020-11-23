using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrdersReportApp.Models.Order
{
    public interface IOrderDataAccess
    {
        List<Order> GetOrders();
        Task<List<Order>> GetOrdersAsync();
        void AddOrder(Order order);
        Task AddOrderAsync(Order order);
        void UpdateOrder(Order order);
        Task UpdateOrderAsync(Order order);
        void RemoveOrder(Order order);
        Task RemoveOrderAsync(Order order);
    }
}
