using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrdersReportApp.ViewModels;

namespace OrdersReportApp.Models.Order
{
    public class OrderValidator : IOrderValidator
    {
        public void PositivePriseChecking(ModelStateDictionary modelState, UpdatingOrderViewModel order)
        {
            if (order.Price < 0)
                modelState.AddModelError("Price", "Общая сумма не может быть отрицательной");
        }
    }
}
