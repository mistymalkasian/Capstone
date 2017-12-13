using PaulyMacs.Models;
using PaulyMacs.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaulyMacs.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session["cart"] as List<CartViewModel> ?? new List<CartViewModel>();

            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty. Fill it with delicious food!";
                return View();
            }

            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.CartTotal;
            }

            ViewBag.GrandTotal = total;

            return View(cart);
        }

        public ActionResult CartPartial()
        {
            CartViewModel model = new CartViewModel();

            int qty = 0;
            decimal price = 0m;

            if (Session["cart"] != null)
            {
                var list = (List<CartViewModel>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.MenuItemPrice;
                }

                model.Quantity = qty;
                model.MenuItemPrice = price;
            }
            else
            {
                model.Quantity = 0;
                model.MenuItemPrice = 0m;
            }

            return PartialView(model);
        }

        public ActionResult AddToCartPartial(int id)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel> ?? new List<CartViewModel>();

            CartViewModel model = new CartViewModel();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                MenuItem item = db.MenuItems.Find(id);

                var itemInCart = cart.FirstOrDefault(x => x.MenuItemId == id);

                if (itemInCart == null)
                {
                    cart.Add(new CartViewModel() { MenuItemId = item.MenuItemId, MenuItemName = item.ItemName, Quantity = 1, MenuItemPrice = item.ItemPrice, Image = item.ImageName });

                }
                else
                {
                    itemInCart.Quantity++;
                }
            }

            int qty = 0;
            decimal price = 0m;

            foreach(var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.MenuItemPrice;
            }

            model.Quantity = qty;
            model.MenuItemPrice = price;

            Session["cart"] = cart;


                return PartialView(model);
            }

        //GET: /Cart/IncrementItem
        public JsonResult IncrementItem(int itemId)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                CartViewModel model = cart.FirstOrDefault(x => x.MenuItemId == itemId);

                model.Quantity++;

                var result = new { qty = model.Quantity, price = model.MenuItemPrice };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        //GET: /Cart/DecrementItem
        public JsonResult DecrementItem(int itemId)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                CartViewModel model = cart.FirstOrDefault(x => x.MenuItemId == itemId);

                if (model.Quantity > 1)
                {
                    model.Quantity--;
                }
                else
                {
                    model.Quantity = 0;
                    cart.Remove(model);
                }
               

                var result = new { qty = model.Quantity, price = model.MenuItemPrice };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public void DeleteItem(int itemId)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                CartViewModel model = cart.FirstOrDefault(x => x.MenuItemId == itemId);

                cart.Remove(model);
            }

        }

        public ActionResult PayPalPartial()
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;

            return PartialView(cart);
        }

        //POST: /Cart/PlaceOrder
        [HttpPost]
        public void PlaceOrder()
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;

            string username = User.Identity.Name;
            

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Order order = new Order();

                var user = db.Users.FirstOrDefault(x => x.UserName == username);
                string userId = user.Id;

                int orderId = order.OrderId;
                
                foreach (var item in cart)
                {
                    order.UserId = userId;
                    order.OrderContents = item.MenuItemName;
                    order.OrderTotalPrice = item.MenuItemPrice;
                }

               
                order.OrderDate = DateTime.Now;
                order.isOrderOpen = true;

                db.Orders.Add(order);
                db.SaveChanges();

            }

            //MAYBE SEND AN EMAIL TO THE ADMIN IF I DECIDE I WANT TO, BUT FOR NOW WILL JUST SHOW OPEN ORDERS IN AN EMPLOYEE VIEW

            Session["cart"] = null;
            
        }

    }
 }