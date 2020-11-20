using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersReportApp.Models.Order
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
