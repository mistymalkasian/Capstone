using PaulyMacs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaulyMacs.Views.ViewModels
{
    public class CartViewModel
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal MenuItemPrice { get; set; }
        public decimal CartTotal { get { return Quantity * MenuItemPrice; } }
        public string Image { get; set; }

        //public string Message { get; set; }

        //public IEnumerable<MenuItem> CartItems { get; set; }


    }
}