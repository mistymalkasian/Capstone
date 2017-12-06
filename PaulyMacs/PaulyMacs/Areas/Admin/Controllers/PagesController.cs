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

                Pages page = new Pages();

                page.Title = model.Title;

                if(string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {

                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }

                page.Slug = slug;
                page.Body = model.Body;
                page.HasSidebar = model.HasSidebar;
                page.Sorting = 100;

                db.Pages.Add(page);
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "You have added a new page.";
            return RedirectToAction("AddPage");
        }

        // GET: Admin/Pages/EditPage/id

        [HttpGet]

        public ActionResult EditPage(int id)
        {
            PageViewModel model;

            using (Db db = new Db())
            {
                Pages page = db.Pages.Find(id);

                if (page == null)
                {
                    return Content("The page does not exist.");
                }

                model = new PageViewModel(page);
            }

                return View(model);
        }


        // GET: Admin/Pages/EditPage/id

        [HttpPost]

        public ActionResult EditPage(PageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Models.Db())
            {
                int id = model.PagesId;

                string slug = "home";

                Pages page = db.Pages.Find(id);

                page.Title = model.Title;

                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                if (db.Pages.Where(x => x.PagesId != id).Any(x => x.Title == model.Title) ||
                   db.Pages.Where(x => x.PagesId != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }

                page.Slug = slug;
                page.Body = model.Body;
                page.HasSidebar = model.HasSidebar;
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Success! You have edited the page.";

                return RedirectToAction("EditPage");
        }


        // GET: Admin/Pages/PageDetails/id

        public ActionResult PageDetails(int id)
        {

            PageViewModel model;

            using (Db db = new Db())
            {
                Pages page = db.Pages.Find(id);

                if (page == null)
                {
                    return Content("The page does not exist.");
                }

                model = new PageViewModel(page);
            }

            return View(model);
        }


    }
}