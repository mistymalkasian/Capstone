using PaulyMacs.Areas.Admin.Models;
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


        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {

            string id;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Categories.Any(x => x.Name == catName))
                    return "titletaken";

                Category category = new Category();

                category.Name = catName;
                category.Slug = catName.Replace(" ", "-").ToLower();
                category.Sorting = 100;

                db.Categories.Add(category);
                db.SaveChanges();

                id = category.CategoryId.ToString();
            }

            return id;
        }

    }
}