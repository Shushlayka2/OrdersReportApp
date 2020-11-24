using System.Collections.Generic;

/// <summary>
/// Class for wrapping orders data on uploading dataTable on Client
/// </summary>
namespace OrdersReportApp.Models.Order.Models
{
    public class OrdersTable
    {
        public List<Order> Data { get; set; }

        public OrdersTable(List<Order> data)
        {
            Data = data;
        }
    }
}
