using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OrdersReportApp.Models
{
    public enum Status
    {
        Success,
        Fail
    }

    public class RequestState
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public ModelStateDictionary ModelState { get; set; }

        public RequestState(Status status, string message, ModelStateDictionary modelState = null)
        {
            Status = status;
            Message = message;
            ModelState = modelState;
        }
    }
}
