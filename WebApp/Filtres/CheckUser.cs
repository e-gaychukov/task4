using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;


namespace WebApp.Filtres
{
    public class CheckUser : FilterAttribute, IActionFilter
    {
        public CheckUser() { }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            RedirectBannedUsers(filterContext.HttpContext, () => { filterContext.Result = null; });
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RedirectBannedUsers(filterContext.HttpContext, () => { filterContext.Result = null; });
        }

        public void RedirectBannedUsers(HttpContextBase context, Action extraActions)
        {
            if (GetCurrentUser(context) == null || GetCurrentUser(context).LockoutEnabled)
            {
                extraActions();
                context.GetOwinContext().Authentication.SignOut();
                context.Response.RedirectToRoute("Default", new { action = "Index", controller = "Home" });
            }
        }

        public ApplicationUser GetCurrentUser(HttpContextBase context)
        {
            var userManager = context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return userManager.FindByNameAsync(context.User.Identity.Name).Result;
        }
    }
}