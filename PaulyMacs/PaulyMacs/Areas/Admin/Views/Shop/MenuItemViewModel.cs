using PaulyMacs.Areas.Admin.Models;
using PaulyMacs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaulyMacs.Areas.Admin.ViewModels
{
    public class MenuItemViewModel
    {


        public MenuItemViewModel()
        {

        }

        public MenuItemViewModel(MenuItem row)
        {
            MenuItemId = row.MenuItemId;
            ItemName = row.ItemName;
            Slug = row.Slug;
            ItemPrice = row.ItemPrice;
            ItemDescription = row.ItemDescription;
            CategoryName = row.CategoryName;
            ImageName = row.ImageName;
            CategoryId = row.CategoryId;
        }



        public int MenuItemId { get; set; }

        [Required]
        public string ItemName { get; set; }

        public string Slug { get; set; }

        public decimal ItemPrice { get; set; }

        [Required]
        public string ItemDescription { get; set; }

        public string CategoryName { get; set; }

        public string ImageName { get; set; }


        //Foreign Key
        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public IEnumerable<string> GalleryImages { get; set; }
    }
}