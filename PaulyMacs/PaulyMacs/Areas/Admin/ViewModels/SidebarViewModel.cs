using PaulyMacs.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaulyMacs.Areas.Admin.ViewModels
{
    public class SidebarViewModel
    {

        public SidebarViewModel()
        {
                
        }

        public SidebarViewModel(Sidebar row)
        {
            Id = row.SidebarId;
            Body = row.Body;
        }

        public int Id { get; set; }

        [AllowHtml]
        public string Body { get; set; }

    }
}