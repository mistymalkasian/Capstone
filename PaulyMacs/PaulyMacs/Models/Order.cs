﻿using System;
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
        public string OrderContents { get; set; }

        [DataType(DataType.Currency)]
        public decimal OrderTotalPrice { get; set; }

        //Foreign Key

        public int CustomerId { get; set; }
        
        public Customer Customer { get; set; }
    }
}