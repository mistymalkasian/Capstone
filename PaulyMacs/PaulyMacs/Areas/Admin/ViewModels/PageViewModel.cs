﻿using PaulyMacs.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaulyMacs.ViewModels
{
    public class PageViewModel
    {
        public PageViewModel()
        {

        }

        public PageViewModel(Pages row)
        {
            PagesId = row.PagesId;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;
        }


        [Key]
        public int PagesId { get; set; }

        [Required]
        [StringLength (50, MinimumLength = 3)]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [AllowHtml]
        public string Body { get; set; }

        public int Sorting { get; set; }

        public bool HasSidebar { get; set; }

    }
}