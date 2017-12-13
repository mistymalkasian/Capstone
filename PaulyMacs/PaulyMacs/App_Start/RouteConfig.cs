using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PaulyMacs
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute("Cart", "Cart/{action}/{id}", new { controller = "Cart", action = "Index", id = UrlParameter.Optional }, new[] { "PaulyMacs.Controllers" });
            routes.MapRoute("Shop", "Shop/{action}/{name}", new { controller = "Shop", action = "Index", name = UrlParameter.Optional }, new[] { "PaulyMacs.Controllers" });
            routes.MapRoute("SidebarPartial", "Pages/SidebarPartial", new { controller = "Pages", action = "SidebarPartial" }, new[] { "PaulyMacs.Controllers" });
            routes.MapRoute("PagesMenuPartial", "Pages/PagesMenuPartial", new { controller = "Pages", action = "PagesMenuPartial" }, new[] { "PaulyMacs.Controllers" });
            routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" }, new[] { "PaulyMacs.Controllers" });
            routes.MapRoute("Default", "", new { controller = "Pages", action = "Index" }, new[] { "PaulyMacs.Controllers" });
            //routes.MapRoute("Orders", "Areas/Admin/Orders", new { controller = "Shop", action = "Orders" }, new[] { "PaulyMacs.Controllers" });




            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                
            );

            routes.MapRoute(
          name: "Contact",
          url: "Home/Contact",
          defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional }

      );

            routes.MapRoute(
    name: "Sent",
    url: "Home/Sent",
    defaults: new { controller = "Home", action = "Sent", id = UrlParameter.Optional }

);


            routes.MapRoute(
              name: "Account",
              url: "Account/Register",
              defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
          
          );

            routes.MapRoute(
             name: "Login",
             url: "Account/Login",
             defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }

         );

            routes.MapRoute(
          name: "LogOff",
          url: "Account/LogOff",
          defaults: new { controller = "Account", action = "LogOff", id = UrlParameter.Optional }

      );



        }
    }
}
