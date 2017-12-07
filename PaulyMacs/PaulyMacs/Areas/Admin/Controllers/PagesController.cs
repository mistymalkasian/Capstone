using PaulyMacs.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulyMacs.Areas.Admin.Models;
using PaulyMacs.Areas.Admin.ViewModels;
using PaulyMacs.Models;

namespace PaulyMacs.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageViewModel> PagesList;

            using (ApplicationDbContext Db = new ApplicationDbContext())
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

            using (ApplicationDbContext db = new ApplicationDbContext())
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

            using (ApplicationDbContext db = new ApplicationDbContext())
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

            using (ApplicationDbContext db = new ApplicationDbContext())
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

            using (ApplicationDbContext db = new ApplicationDbContext())
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


        // GET: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Pages page = db.Pages.Find(id);

                db.Pages.Remove(page);

                db.SaveChanges();         
                    
            }
                return RedirectToAction("Index");
        }


        // GET: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                int count = 1;
                Pages page;

                foreach(var pageId in id)
                {
                    page = db.Pages.Find(pageId);
                    page.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }
        }

        // GET: Admin/Pages/EditSidebar/id

       [HttpGet]
        public ActionResult EditSidebar(int id)
        {
            SidebarViewModel model;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Sidebar sidebar = db.Sidebar.Find(1);

                model = new SidebarViewModel(sidebar);
            }

                return View(model);
        }

        // POST: Admin/Pages/EditSidebar

        [HttpPost]
        public ActionResult EditSidebar(SidebarViewModel model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Sidebar sidebar = db.Sidebar.Find(1);

                sidebar.Body = model.Body;

                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "You have edited the sidebar.";

            return RedirectToAction("EditSidebar");
        }





    }
}