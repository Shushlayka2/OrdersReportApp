using OrdersReportApp.Models.Order.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace OrdersReportApp.ViewModels
{
    public class UpdatingOrderViewModel
    {
        [Required(ErrorMessage = "Не указан идентификатор заказа")]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Не указана сумма заказа")]
        [PositiveValue(ErrorMessage = "Cумма заказа не может быть отрицательной")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Не указана дата заказа")]
        public DateTime? Date { get; set; }
    }
}
