using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaulyMacs.Areas.Admin.ViewModels
{
    public class OrdersForAdminViewModel
    {
        public int OrderNumber { get; set; }
        public string Username { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQty { get; set; }
        public DateTime OrderDate { get; set; }
        public bool isOrderOpen { get; set; }

    }
}