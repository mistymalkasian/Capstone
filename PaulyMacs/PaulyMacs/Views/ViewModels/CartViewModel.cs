using PaulyMacs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaulyMacs.Views.ViewModels
{
    public class CartViewModel
    {
        
        public decimal CartTotal { get; set; }
        public string Message { get; set; }

        public IEnumerable<MenuItem> CartItems { get; set; }


    }
}