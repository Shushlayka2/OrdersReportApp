﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersReportApp.Models.Order
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Не указана сумма заказа")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Не указана дата заказа")]
        public DateTime Date { get; set; }
    }
}
