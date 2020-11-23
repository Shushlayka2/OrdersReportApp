using OrdersReportApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersReportApp.Models.Order
{
    public class OrderDataAccess : IOrderDataAccess
    {
        public DatabaseContext Database { get; }

        public OrderDataAccess(DatabaseContext database)
        {
            Database = database;
        }

        public async Task AddNewOrderAsync(NewOrderViewModel newOrder)
        {
            var order = new Order() 
            {
                Date = newOrder.Date,
                Price = newOrder.Price
            };
            await Database.AddAsync(order);
            await Database.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            Database.Orders.Update(order);
            await Database.SaveChangesAsync();
        }

        public async Task RemoveOrderAsync(Order order)
        {
            Database.Remove(order);
            await Database.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await Task.Run(() => GetOrders());
        }

        public List<Order> GetOrders()
        {
            return Database.Orders.ToList();
        }
    }
}
