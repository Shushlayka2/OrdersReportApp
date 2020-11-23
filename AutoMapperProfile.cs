using AutoMapper;
using OrdersReportApp.Models.Order;
using OrdersReportApp.ViewModels;

namespace OrdersReportApp
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewOrderViewModel, Order>();
            CreateMap<UpdatingOrderViewModel, Order>();
        }
    }
}
