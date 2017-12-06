using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PaulyMacs.Areas.Admin.Models
{
    public class Sidebar
    {

        [Key]
        public int SidebarId { get; set; }

      
        public string Body { get; set; }

    }
}