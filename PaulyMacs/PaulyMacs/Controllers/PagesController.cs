using PaulyMacs.Areas.Admin.Models;
using PaulyMacs.Areas.Admin.ViewModels;
using PaulyMacs.Models;
using PaulyMacs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaulyMacs.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index/[page]
        public ActionResult Index(string page = "")
        {
            if (page == "")
                page = "home";

            PageViewModel model;
            Pages dto;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if(! db.Pages.Any(x => x.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }

            ViewBag.PageTitle = dto.Title;

            if (dto.HasSidebar == true)
            {
                ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "No";
            }

            model = new PageViewModel(dto);

            return View(model);
        }


        public ActionResult PagesMenuPartial()
        {
            List<PageViewModel> pageVMList;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                pageVMList = db.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "home").Select(x => new PageViewModel(x)).ToList();
            }
                return PartialView(pageVMList);
        }

        public ActionResult SidebarPartial()
        {
            SidebarViewModel model;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Sidebar sidebar = db.Sidebars.Find(1);

                model = new SidebarViewModel(sidebar);
            }

                return View(model);
        }
    }
}