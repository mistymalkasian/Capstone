using PaulyMacs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaulyMacs.Views.Shop
{
    public class OrderViewModel
    {


        public OrderViewModel()
        {

        }

        public OrderViewModel(Order row)
        {
            OrderId = row.OrderId;
            UserId = row.UserId;
            OrderDate = row.OrderDate;
        }


        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool isOrderOpen { get; set; }
        public string OrderContents { get; set; }

        public decimal OrderTotalPrice { get; set; }

        public int CustomerId { get; set; }

        public string UserId { get; set; }

    }
}