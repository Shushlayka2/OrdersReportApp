using System;
using System.ComponentModel.DataAnnotations;

namespace OrdersReportApp.Models.Order.Validators
{
    public class PositiveValueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                if (value == null)
                    return false;
                else
                    return (decimal)value > 0;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}
