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
using PaulyMacs.Views.Shop;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace PaulyMacs.Areas.Admin.Controllers
{

    public class ShopController : Controller
    {
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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


        //GET: Admin/Shop/MenuItems
        [Authorize(Roles = "Admin")]
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


        //GET: Admin/Shop/EditMenuItem/id
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditMenuItem(int id)
        {
            MenuItemViewModel model;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                MenuItem item = db.MenuItems.Find(id);

                if(item == null)
                {
                    return Content("That item does not exist.");
                }

                model = new MenuItemViewModel(item);

                model.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "Name");

                model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/MenuItems/" + id + "/Gallery/Thumbs"))
                                               .Select(fn => Path.GetFileName(fn));

           
            }

                return View(model);
        }


        //POST: Admin/Shop/EditMenuItem/id
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditMenuItem(MenuItemViewModel model, HttpPostedFileBase file)
        {
            int id = model.MenuItemId;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "Name");
            }
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/MenuItems/" + id + "/Gallery/Thumbs"))
                                              .Select(fn => Path.GetFileName(fn));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.MenuItems.Where(x => x.MenuItemId != id).Any(x => x.ItemName == model.ItemName))
                {
                    ModelState.AddModelError("", "That item name is taken.");
                    return View(model);
                }
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                MenuItem item = db.MenuItems.Find(id);

                item.ItemName = model.ItemName;
                item.Slug = model.ItemName.Replace(" ", "-").ToLower();
                item.ItemDescription = model.ItemDescription;
                item.ItemPrice = model.ItemPrice;
                item.CategoryId = model.CategoryId;
                item.ImageName = model.ImageName;

                Category catDTO = db.Categories.FirstOrDefault(x => x.CategoryId == model.CategoryId);
                item.CategoryName = catDTO.Name;

                db.SaveChanges();

            }

            TempData["SuccessMessage"] = "You have successfully edited the menu item!";

            #region Image Upload

            if (file != null && file.ContentLength > 0)
            {
                string ext = file.ContentType.ToLower();

                if (ext != "image/jpg" &&
                   ext != "image/jpeg" &&
                   ext != "image/pjpeg" &&
                   ext != "image/gif" &&
                   ext != "image/x-png" &&
                   ext != "image/png"
                    )
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View(model);
                    }
                }


                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));


                var pathString1 = Path.Combine(originalDirectory.ToString(), "MenuItems\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "MenuItems\\" + id.ToString() + "\\Thumbs");

                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file2 in di1.GetFiles())
                    file2.Delete();

                foreach (FileInfo file3 in di2.GetFiles())
                    file3.Delete();

                string imageName = file.FileName;

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    MenuItem item = db.MenuItems.Find(id);
                    item.ImageName = imageName;

                    db.SaveChanges();
                }

                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);

            }

            #endregion

            return RedirectToAction("EditMenuItem");
        }

        //GET: Admin/Shop/DeleteMenuItem/id
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteMenuItem(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                MenuItem item = db.MenuItems.Find(id);
                db.MenuItems.Remove(item);

                db.SaveChanges();
            }

            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            string pathString = Path.Combine(originalDirectory.ToString(), "MenuItems\\" + id.ToString());

            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);

            return RedirectToAction("MenuItems");
        }

        //POST: Admin/Shop/SaveGalleryImages
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            foreach(string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                if (file != null && file.ContentLength > 0)
                {
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                    string pathString1 = Path.Combine(originalDirectory.ToString(), "MenuItems\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(), "MenuItems\\" + id.ToString() + "\\Gallery\\Thumbs");

                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);

                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }

            }

        }

        //POST: Admin/Shop/DeleteImage
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~Images/Uploads/MenuItems/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~Images/Uploads/MenuItems/" + id.ToString() + "/Gallery/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);
        }

        //Get: Admin/Shop/Orders
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Orders()
        {
            List<OrdersForAdminViewModel> ordersForAdmin = new List<OrdersForAdminViewModel>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<OrderViewModel> orders = db.Orders.ToArray().Select(x => new OrderViewModel(x)).ToList();


                foreach (var order in orders)
                {
                    Dictionary<string, int> productsAndQty = new Dictionary<string, int>();


                    decimal total = 0m;

                    List<Order> orderDetailsList = db.Orders.Where(x => x.OrderId == order.OrderId).ToList();

                    ApplicationUser user = db.Users.Where(x => x.Id == order.UserId).FirstOrDefault();

                    string username = user.Email;

                    foreach(var orderDetails in orderDetailsList)
                    {
                        MenuItem item = db.MenuItems.Where(x => x.MenuItemId == orderDetails.MenuItemId).FirstOrDefault();

                        decimal price = item.ItemPrice;

                        string itemName = item.ItemName;

                        productsAndQty.Add(itemName, orderDetails.Quantity);

                        total += orderDetails.Quantity * price;
                    }

                    ordersForAdmin.Add(new OrdersForAdminViewModel()

                    {
                        OrderNumber = order.OrderId,
                        Username = user.Email,
                        Total = total,
                        ProductsAndQty = productsAndQty,
                        OrderDate = order.OrderDate,
                        isOrderOpen = order.isOrderOpen
                        
                                        
                    });

                }

            }

            return View(ordersForAdmin);
        }

        [Authorize(Roles = "Admin, Employee")]
        public ActionResult CompletedOrder()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> SendMsgsAndConfirm(Order order)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            if (ModelState.IsValid)
            {
                //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("andrewandmisty@gmail.com"));
                message.From = new MailAddress("devcodecamptest@gmail.com");
                message.Subject = "Rate Us!";
                message.Body = "Were you happy with the service you received today? <br> Follow this link to tell us how we did: <a href=\"google.com\">Rate my Service</a>";
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "devcodecamptest@gmail.com",
                        Password = "devCodeDawg02"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    await SendText();
                    order.isOrderOpen = false;
                    db.SaveChanges();
                }
            }
            return View("CompletedOrder");
        }

        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult> SendText()
        {
            //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("2629946699@email.uscc.net"));
            message.From = new MailAddress("devcodecamptest@gmail.com");
            message.Subject = "Your order is ready to be picked up at Pauly Mac's!";
            message.Body = "We hope you enjoy your meal, and make sure to rate us!";
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "devcodecamptest@gmail.com",
                    Password = "devCodeDawg02"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return View();
            }
        }
    }
}

