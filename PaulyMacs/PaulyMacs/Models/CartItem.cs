using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PaulyMacs.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal ItemPrice { get; set; }


        //Foreign Keys

        public int CartId { get; set; }
        public Cart Cart { get; set; }

  
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

    }
}