using PaulyMacs.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaulyMacs.ViewModels
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {

        }
        public CategoryViewModel(Category row)
        {
            CategoryId = row.CategoryId;
            Name = row.Name;
            Slug = row.Slug;
            Sorting = row.Sorting;
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}