using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.BusinessLogic.UserAccounts;
using JazMax.Web.ViewModel.UserAccountView;
using JazMax.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace JazMax.Web.Controllers
{
    public class UserController : Controller
    {
        private static CoreUserService obj = new CoreUserService();
        // GET: User
        public ActionResult Index()
        {
            return View(obj.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CoreUserView model)
        {
            if(ModelState.IsValid)
            {
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                ApplicationUserManager _userManager = new ApplicationUserManager(store);
                var manger = _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                string studentNumber = model.EmailAddress;
                string password = "Passw0rd123*";
                var user = new ApplicationUser() { Email = studentNumber, UserName = studentNumber };
                var usmanger = manger.Create(user, password);

                //AddUserToRole Based On My Role Type BOIIIII
                //We Don't Use IdentityManager for this rather piggy bank of EnityFramework for my insert
                //Why? Because I CAN!
                obj.AddUserToAspUserRole(user.Id, obj.GetRoleGUID(model.CoreUserTypeId));

                //DO: MAIN CREATE
                obj.CreateNewCoreUser(model);

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}