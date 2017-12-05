using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaulyMacs.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }

        public string ItemPicture { get; set; }

        [Required, Display(Name = "Item name")]
        public string ItemName { get; set; }

        [Required, Display(Name = "Item price")]
        public decimal ItemPrice { get; set; }

        [Required, Display(Name = "Item Description")]
        public string ItemDescription { get; set; }

        

       

    }
}