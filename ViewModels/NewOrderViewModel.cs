using System.ComponentModel.DataAnnotations;

namespace OrdersReportApp.ViewModels
{
    public class NewOrderViewModel
    {
        [Required(ErrorMessage = "Не указана сумма заказа")]
        public string Price { get; set; }

        [Required(ErrorMessage = "Не указана дата заказа")]
        public string Date { get; set; }
    }
}
