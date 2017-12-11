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
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            List<CategoryViewModel> categoryVMList;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                categoryVMList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryViewModel(x)).ToList();
            }
                return PartialView(categoryVMList);
        }

        // GET: Shop/Category/name
        public ActionResult Category(string name)
        {
            List<MenuItemViewModel> itemVMList;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Category category = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = category.CategoryId;

                itemVMList = db.MenuItems.ToArray().Where(x => x.CategoryId == catId).Select(x => new MenuItemViewModel(x)).ToList();

                var itemCat = db.MenuItems.Where(x => x.CategoryId == catId).FirstOrDefault();
                ViewBag.CategoryName = itemCat.CategoryName;
            }

                return View(itemVMList);
        }
    }
}