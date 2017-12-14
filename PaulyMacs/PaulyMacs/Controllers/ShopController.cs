using PaulyMacs.Areas.Admin.Models;
using PaulyMacs.Areas.Admin.ViewModels;
using PaulyMacs.Models;
using PaulyMacs.ViewModels;
using PaulyMacs.Views.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
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

        // GET: Shop/menuitem-details/name
        [ActionName("menuitem-details")]
        public ActionResult MenuItemDetails(string name)
        {
            MenuItemViewModel model;
            MenuItem item;
            int id = 0;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if(!db.MenuItems.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                item = db.MenuItems.Where(x => x.Slug == name).FirstOrDefault();

                id = item.MenuItemId;

                model = new MenuItemViewModel(item);
          
            }

            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/MenuItems/" + id + "/Gallery/Thumbs"))
                                              .Select(fn => Path.GetFileName(fn));

            return View("MenuItemDetails", model);
        }

        [Authorize(Roles="Admin, Employee")]
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

                    foreach (var orderDetails in orderDetailsList)
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

        [Authorize(Roles="Admin, Employee")]
        public ActionResult CompletedOrder()
        {
            return View();
        }

        [Authorize(Roles="Employee, Admin")]
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

        [Authorize(Roles="Admin, Employee")]
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