using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PaulyMacs.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool isOrderOpen { get; set; }
        public IEnumerable<string> OrderContents { get; set; }

        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal OrderTotalPrice { get; set; }

        //Foreign Keys

        
        public string UserId { get; set; }

        public int MenuItemId { get; set; }

    }
}