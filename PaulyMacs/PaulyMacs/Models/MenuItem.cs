using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaulyMacs.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }

    }
}