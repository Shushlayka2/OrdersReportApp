using System;
using System.ComponentModel.DataAnnotations;

namespace OrdersReportApp.ViewModels
{
    public class UpdatingOrderViewModel
    {
        [Required(ErrorMessage = "Потерян идентификатор заказа")]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Не указана сумма заказа")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Не указана дата заказа")]
        public DateTime? Date { get; set; }
    }
}
