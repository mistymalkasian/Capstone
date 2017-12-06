using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaulyMacs.Areas.Admin.Models
{
    public class Pages
    {
        [Key]
        public int PagesId { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string Body { get; set; }

        public int Sorting { get; set; }

        public bool HasSidebar { get; set; }
    }
}