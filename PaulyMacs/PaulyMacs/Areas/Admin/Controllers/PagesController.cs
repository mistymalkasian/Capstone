using PaulyMacs.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulyMacs.Areas.Admin.Models;

namespace PaulyMacs.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageViewModel> PagesList;

            using (Db Db = new Db())
            {
                PagesList = Db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageViewModel(x)).ToList();
            }

                return View(PagesList);
        }


        // GET: Admin/Pages/AddPage

        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage

        [HttpPost]
        public ActionResult AddPage(PageViewModel model)
        {

            if(! ModelState.IsValid)

            {
                return View(model);
            }

            using (Db db = new Models.Db())
            {
                string slug;

                Pages pages = new Pages();

                pages.Title = model.Title;

                if(string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                
                }
                else
                {

                }
            }


                return View();
        }
    }
}