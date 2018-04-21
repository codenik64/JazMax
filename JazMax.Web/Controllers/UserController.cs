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
using JazMax.Core.SystemHelpers.Model;

namespace JazMax.Web.Controllers
{
    public class UserController : Controller
    {
        private static CoreUserService obj = new CoreUserService();
        private static JazMaxIdentityHelper _helper = new JazMaxIdentityHelper();

        #region Get All
        public ActionResult Index()
        {
            return View(obj.GetAllSystemUsers( new List<bool> { true, false }));
        }
        #endregion

        #region CoreUser Details
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(obj.GetUserDetails((int)id));
        }
        #endregion

        #region Create Core User Global
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(string FirstName, string MiddleName, string LastName, string IDNumber, string PhoneNumber, string CellPhone, string EmailAddress, string GenderId, string CoreUserTypeId, string CapturePAView_provinceId, string CaptureTeamLeader_provinceId, string dropBranch)
        {
            try
            {
                #region Capture CoreUser Details
                JazMax.Web.ViewModel.UserAccountView.CoreUserView m = new ViewModel.UserAccountView.CoreUserView()
                {
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    LastName = LastName,
                    EmailAddress = EmailAddress,
                    IsActive = true,
                    IDNumber = IDNumber,
                    PhoneNumber = PhoneNumber,
                    CellPhone = CellPhone
                };
                m.EmailAddress = EmailAddress;

                m.GenderId = Convert.ToInt32(GenderId);
                m.CoreUserTypeId = Convert.ToInt32(CoreUserTypeId);

                CapturePAView mm = new CapturePAView()
                {
                    provinceId = Convert.ToInt16(CapturePAView_provinceId)
                };
                CaptureTeamLeader zz = new CaptureTeamLeader()
                {
                    provinceId = Convert.ToInt32(CaptureTeamLeader_provinceId)
                };
                CaptureAgent yy = new CaptureAgent();
                try
                {


                    yy.BranchId = Convert.ToInt32(dropBranch);
                }
                catch
                {
                    yy.BranchId = 0;
                }
                m.CapturePAView = mm;
                m.CaptureTeamLeader = zz;
                m.CaptureAgent = yy;
                obj.CreateNewCoreUser(m);
                #endregion

                #region Capture AspUser and Role
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                ApplicationUserManager _userManager = new ApplicationUserManager(store);
                var manger = _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                string studentNumber = EmailAddress;
                string password = "Passw0rd123*";
                var user = new ApplicationUser() { Email = studentNumber, UserName = studentNumber };
                var usmanger = manger.Create(user, password);

                obj.AddUserToAspUserRole(user.Id, obj.GetRoleGUID(Convert.ToInt16(CoreUserTypeId)));
                #endregion

                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
                return Json(new { Result = "Fail", Message = "Error, Could not save user. Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Create Agent By TeamLeader
        public ActionResult CreateAgent()
        {
            return View();
        }

        public ActionResult SaveAgent(string FirstName, string MiddleName, string LastName, string IDNumber, string PhoneNumber, string CellPhone, string EmailAddress, string GenderId)
        {
            try
            {
                #region Capture CoreUser Details
                TeamLeaderInfomation team = new TeamLeaderInfomation();

                JazMaxIdentityHelper.UserName = User.Identity.Name;
                team = JazMaxIdentityHelper.GetTeamLeadersInfoNew();

                CoreUserView m = new CoreUserView()
                {
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    LastName = LastName,
                    EmailAddress = EmailAddress,
                    IsActive = true,
                    IDNumber = IDNumber,
                    PhoneNumber = PhoneNumber,
                    CellPhone = CellPhone
                };
                m.EmailAddress = EmailAddress;

                m.GenderId = Convert.ToInt32(GenderId);
                m.CoreUserTypeId = Convert.ToInt32(4);
                
                #region Not Required
                CapturePAView mm = new CapturePAView()
                {
                    provinceId = 0
                };
                CaptureTeamLeader zz = new CaptureTeamLeader()
                {
                    provinceId = 0
                };
                #endregion

                CaptureAgent yy = new CaptureAgent()
                {
                    BranchId = team.CoreBranchId
                };

                m.CapturePAView = mm;
                m.CaptureTeamLeader = zz;
                m.CaptureAgent = yy;
                obj.CreateNewCoreUser(m);
                #endregion

                #region Capture AspUser and Role
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                ApplicationUserManager _userManager = new ApplicationUserManager(store);
                var manger = _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                string studentNumber = EmailAddress;
                string password = "Passw0rd123*";
                var user = new ApplicationUser() { Email = studentNumber, UserName = studentNumber };
                var usmanger = manger.Create(user, password);

                obj.AddUserToAspUserRole(user.Id, obj.GetRoleGUID(Convert.ToInt16(4)));
                #endregion

                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
                return Json(new { Result = "Fail", Message = "Error, Could not save user. Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Create Agent By PA
        public ActionResult CreateAnAgent()
        {
            return View();
        }

        public ActionResult SaveAnAgent(string FirstName, string MiddleName, string LastName, string IDNumber, string PhoneNumber, string CellPhone, string EmailAddress, string GenderId, string dropBranch)
        {
            try
            {
                #region Capture CoreUser Details

                JazMax.Web.ViewModel.UserAccountView.CoreUserView m = new ViewModel.UserAccountView.CoreUserView()
                {
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    LastName = LastName,
                    EmailAddress = EmailAddress,
                    IsActive = true,
                    IDNumber = IDNumber,
                    PhoneNumber = PhoneNumber,
                    CellPhone = CellPhone
                };
                m.EmailAddress = EmailAddress;
                m.BranchIdCapture = Convert.ToInt16(dropBranch);

                m.GenderId = Convert.ToInt32(GenderId);
                m.CoreUserTypeId = Convert.ToInt32(4);

                #region Not Required
                CapturePAView mm = new CapturePAView()
                {
                    provinceId = 0
                };
                CaptureTeamLeader zz = new CaptureTeamLeader()
                {
                    provinceId = 0
                };
                #endregion

                CaptureAgent yy = new CaptureAgent()
                {
                    BranchId = Convert.ToInt16(dropBranch)
                };

                m.CapturePAView = mm;
                m.CaptureTeamLeader = zz;
                m.CaptureAgent = yy;
                obj.CreateNewCoreUser(m);
                #endregion

                #region Capture AspUser and Role
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                ApplicationUserManager _userManager = new ApplicationUserManager(store);
                var manger = _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                string studentNumber = EmailAddress;
                string password = "Passw0rd123*";
                var user = new ApplicationUser() { Email = studentNumber, UserName = studentNumber };
                var usmanger = manger.Create(user, password);

                obj.AddUserToAspUserRole(user.Id, obj.GetRoleGUID(Convert.ToInt16(4)));
                #endregion

                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
                return Json(new { Result = "Fail", Message = "Error, Could not save Agent. Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
           return View(obj.GetUserDetails((int)id));
        }
       

        #region Update CoreUser
        public ActionResult UpdateCoreUser(string coreUserId, string FirstName, string LastName, string MiddleName, string PhoneNumber, string CellPhone, string IdNumber)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                CoreUserDetails m = new CoreUserDetails()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    MiddleName = MiddleName,
                    PhoneNumber = PhoneNumber,
                    CellPhone = CellPhone,
                    IDNumber = IdNumber
                };
                obj.UpdateCoreUser(Convert.ToInt32(coreUserId), m, JazMaxIdentityHelper.GetCoreUserId());
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Branch could not be updated" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region AJAX Calls
        public ActionResult GetBranchForProvince(int Id)
        {
            return Json(_helper.GetBranchesBasedOnProvince(Id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBranchsForAgentProvince()
        {
            UserInformation a = JazMaxIdentityHelper.GetPAUserInformation(User.Identity.Name);
            return Json(_helper.GetBranchesBasedOnProvince(a.ProvinceId), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(obj.GetUserDetails((int)id));
        }


    }
}