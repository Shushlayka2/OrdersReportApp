using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrdersReportApp.ViewModels;

namespace OrdersReportApp.Models.Order
{
    public interface IOrderValidator
    {
        void PositivePriseChecking(ModelStateDictionary modelState, UpdatingOrderViewModel order);
    }
}
