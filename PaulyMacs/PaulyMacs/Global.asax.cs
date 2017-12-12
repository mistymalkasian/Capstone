using PaulyMacs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PaulyMacs
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_AuthenticateRequest()
        //{

        //    if(User == null) { return; }

        //    string username = Context.User.Identity.Name;

        //    string[] roles = null;

        //    using (ApplicationDbContext db = new ApplicationDbContext())
        //    {
        //        ApplicationUser user = db.Users.FirstOrDefault(x => x.UserName == username);

        //        roles = db.Roles.Where(x => x.Id == user.Id).Select(x => x.Name).ToArray();

        //    }
        //    IIdentity userIdentity = new GenericIdentity(username);
        //    IPrincipal newUserObj = new GenericPrincipal(userIdentity, roles);

        //    Context.User = newUserObj;

        //}
    }
}
