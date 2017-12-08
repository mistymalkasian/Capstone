using PaulyMacs.Areas.Admin.Models;
using PaulyMacs.Areas.Admin.ViewModels;
using PaulyMacs.Models;
using PaulyMacs.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;

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


        // POST: Admin/Shop/ReorderCategories

        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                int count = 1;
                Category category;

                foreach (var catId in id)
                {
                    category = db.Categories.Find(catId);
                    category.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }
        }


        // GET: Admin/Shop/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Category category = db.Categories.Find(id);

                db.Categories.Remove(category);

                db.SaveChanges();

            }
            return RedirectToAction("Categories");
        }


        // POST: Admin/Shop/RenameCategory/id
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Categories.Any(x => x.Name == newCatName))
                    return "titletaken";
                
                Category category = db.Categories.Find(id);

                category.Name = newCatName;
                category.Slug = newCatName.Replace(" ", "-").ToLower();

                db.SaveChanges();
            }

            return "inconsequentialstring";
        }


        // GET: Admin/Shop/AddMenuItem
        [HttpGet]
        public ActionResult AddMenuItem()
        {
            MenuItemViewModel model = new MenuItemViewModel();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "Name");

            }

                return View(model);
        }

        // POST: Admin/Shop/AddMenuItem
        [HttpPost]
        public ActionResult AddMenuItem(MenuItemViewModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "Name");
                    return View(model);
                }
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.MenuItems.Any(x => x.ItemName == model.ItemName))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "Name");
                    ModelState.AddModelError("", "That menu item name is taken.");
                    return View(model);
                }

            }

            int id;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                MenuItem item = new MenuItem();

                item.ItemName = model.ItemName;
                item.Slug = model.ItemName.Replace(" ", "-").ToLower();
                item.ItemDescription = model.ItemDescription;
                item.ItemPrice = model.ItemPrice;
                item.CategoryId = model.CategoryId;

                Category category = db.Categories.FirstOrDefault(x => x.CategoryId == model.CategoryId);
                item.CategoryName = category.Name;

                db.MenuItems.Add(item);
                db.SaveChanges();

                id = item.MenuItemId;
            }

            TempData["SuccessMessage"] = "You have successfully added an item to the menu!";

            #region Upload Image

            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "MenuItems");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "MenuItems\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "MenuItems\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "MenuItems\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "MenuItems\\" + id.ToString() + "\\Gallery\\Thumbs");


            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            if (file != null && file.ContentLength > 0)
            {
                string ext = file.ContentType.ToLower();

                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "Name");
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View(model);
                    }
                }

                string imageName = file.FileName;

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    MenuItem item = db.MenuItems.Find(id);

                    item.ImageName = imageName;

                    db.SaveChanges();
                }

                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            #endregion

            return RedirectToAction("AddMenuItem");
        }


        //POST: Admin/Shop/MenuItems
        public ActionResult MenuItems(int? page, int? catId)
        {
            List<MenuItemViewModel> listOfItemVM;

            var pageNumber = page ?? 1;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                listOfItemVM = db.MenuItems.ToArray()
                              .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                              .Select(x => new MenuItemViewModel(x))
                              .ToList();

                ViewBag.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "Name");

                ViewBag.SelectedCat = catId.ToString();
            }

            var onePageOfProducts = listOfItemVM.ToPagedList(pageNumber, 3);

            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(listOfItemVM);
        }
    }
}

