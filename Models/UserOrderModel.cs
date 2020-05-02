using E_Ticaret.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Ticaret.Models
{
    public class UserOrderModel
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; }
        public double Total { get; set; }
        public DateTime Orderdate { get; set; }
        public EnumOrderState OrderState { get; set; }
    }
}