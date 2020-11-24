using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersReportApp.Models.Order
{
    public class OrderDataAccess : IOrderDataAccess
    {
        protected DatabaseContext Database { get; }

        public OrderDataAccess(DatabaseContext database)
        {
            Database = database;
        }

        public List<Order> GetOrders()
        {
            return Database.Orders.ToList();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await Task.Run(() => GetOrders());
        }

        public void AddOrder(Order order)
        {
            Database.Add(order);
            Database.SaveChanges();
        }

        public async Task AddOrderAsync(Order order)
        {
            await Database.AddAsync(order);
            await Database.SaveChangesAsync();
        }

        public void UpdateOrder(Order order)
        {
            Database.Orders.Update(order);
            Database.SaveChanges();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            Database.Orders.Update(order);
            await Database.SaveChangesAsync();
        }

        public void RemoveOrder(Order order)
        {
            Database.Remove(order);
            Database.SaveChangesAsync();
        }

        public async Task RemoveOrderAsync(Order order)
        {
            Database.Remove(order);
            await Database.SaveChangesAsync();
        }
    }
}
