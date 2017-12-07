using PaulyMacs.Models;
using PaulyMacs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaulyMacs.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            List<CategoryViewModel> CategoryVMList;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                CategoryVMList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryViewModel(x)).ToList();
            }

                return View(CategoryVMList);
        }
    }
}