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
using JazMax.Core.SystemHelpers;

namespace JazMax.Web.Controllers
{
    public class UserController : Controller
    {
        private static CoreUserService obj = new CoreUserService();
        private static JazMaxIdentityHelper _helper = new JazMaxIdentityHelper();
        // GET: User
        public ActionResult Index()
        {
            return View(obj.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Create(CoreUserView model)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
        //        ApplicationUserManager _userManager = new ApplicationUserManager(store);
        //        var manger = _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        string studentNumber = model.EmailAddress;
        //        string password = "Passw0rd123*";
        //        var user = new ApplicationUser() { Email = studentNumber, UserName = studentNumber };
        //        var usmanger = manger.Create(user, password);

        //        //AddUserToRole Based On My Role Type BOIIIII
        //        //We Don't Use IdentityManager for this rather piggy bank of EnityFramework for my insert
        //        //Why? Because I CAN!
        //        obj.AddUserToAspUserRole(user.Id, obj.GetRoleGUID(model.CoreUserTypeId));

        //        //DO: MAIN CREATE
        //        obj.CreateNewCoreUser(model);

        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}

        //TO DO: Add validation and Create AspUser and Role!
        public ActionResult Save(string FirstName, string MiddleName, string LastName, string IDNumber, string PhoneNumber, string CellPhone, string EmailAddress, string GenderId, string CoreUserTypeId, string CapturePAView_provinceId, string CaptureTeamLeader_provinceId, string dropBranch)
        {
            string val = CapturePAView_provinceId;
            JazMax.Web.ViewModel.UserAccountView.CoreUserView m = new ViewModel.UserAccountView.CoreUserView();
            m.FirstName = FirstName;
            m.MiddleName = MiddleName;
            m.LastName = LastName;
            m.EmailAddress = EmailAddress;
            m.IsActive = true;
            m.IDNumber = IDNumber;
            m.PhoneNumber = PhoneNumber;
            m.CellPhone = CellPhone;
            m.EmailAddress = EmailAddress;

            m.GenderId = Convert.ToInt32(GenderId);
            m.CoreUserTypeId = Convert.ToInt32(CoreUserTypeId);

            CapturePAView mm = new CapturePAView();
            mm.provinceId = Convert.ToInt16(CapturePAView_provinceId);

            CaptureTeamLeader zz = new CaptureTeamLeader();
            zz.provinceId = Convert.ToInt32(CaptureTeamLeader_provinceId);

            CaptureAgent yy = new CaptureAgent();
            yy.BranchId = Convert.ToInt32(dropBranch);

            m.CapturePAView = mm;
            m.CaptureTeamLeader = zz;
            m.CaptureAgent = yy;


            obj.CreateNewCoreUser(m);
            return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBranchForProvince(int Id)
        {
            return Json(_helper.GetBranchesBasedOnProvince(Id), JsonRequestBehavior.AllowGet);
        }
    }
}