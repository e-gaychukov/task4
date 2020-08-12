using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using WebApp.Filtres;
using System.Reflection;


namespace WebApp.Controllers
{
    [Authorize]
    [CheckUser]
    public class HomeController : Controller
    {
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        public ActionResult Index()
        {
            return View(UserManager.Users.ToArray());
        }
        public ActionResult BlockUsers(string[] selected)
        {
            Array.ForEach(selected, id => UserManager.SetLockoutEnabledAsync(id, true).Wait());
            return View("Index", UserManager.Users.ToArray());
        }
        public ActionResult UnblockUsers(string[] selected)
        {
            Array.ForEach(selected, id => UserManager.SetLockoutEnabledAsync(id, false).Wait());
            return View("Index", UserManager.Users.ToArray());
        }
        public ActionResult DeleteUsers(string[] selected)
        {
            Array.ForEach(selected, id => UserManager.DeleteAsync(UserManager.FindByIdAsync(id).Result).Wait());
            return View("Index", UserManager.Users.ToArray());
        }
        public ApplicationUser GetCurrentUser()
        {
            var userManager = HttpContext.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
        }
    }
}