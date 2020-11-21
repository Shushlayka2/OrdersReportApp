using OrdersReportApp.ViewModels;
using System.Threading.Tasks;

namespace OrdersReportApp.Services
{
    public interface IOrdersReporter
    {
        string CreateReport(ReportViewModel reportViewModel);
        Task<string> CreateReportAsync(ReportViewModel reportViewModel);
    }
}
