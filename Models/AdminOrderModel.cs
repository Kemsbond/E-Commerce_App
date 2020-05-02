using E_Ticaret.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    public class AdminOrderModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OrderNumber { get; set; }
        public double Total { get; set; }
        public DateTime Orderdate { get; set; }
        public EnumOrderState OrderState { get; set; }
        public int Count { get; set; }

    }
}