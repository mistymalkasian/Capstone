using PaulyMacs.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PaulyMacs.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }

        [Required, Display(Name = "Item name")]
        public string ItemName { get; set; }

        public string Slug { get; set; }

        [Required, Display(Name = "Item price")]
        public decimal ItemPrice { get; set; }

        [Required, Display(Name = "Item Description")]
        public string ItemDescription { get; set; }

        public string CategoryName { get; set; }

        public string ImageName { get; set; }



        //Foreign Key
        public int CategoryId { get; set; }

        public Category Category { get; set; }




    }
}