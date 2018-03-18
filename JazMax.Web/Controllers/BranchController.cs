using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.BusinessLogic.UserAccounts;
using JazMax.Core.SystemHelpers;
using JazMax.Core.SystemHelpers.Model;

namespace JazMax.Web.Controllers
{
    public class BranchController : Controller
    {
        private static CoreBranchService o = new CoreBranchService();
        private static JazMaxIdentityHelper _helper = new JazMaxIdentityHelper();

        // GET: Branch
        #region Get All Branches
        public ActionResult Index()
        {
            return View(o.GetAll());
        }
        #endregion

        #region Create New Branch
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Save(string CoreTeamLeaderId, string ProvinceId, string BranchName, string Phone, string EmailAddress, string StreetAddress, string City, string Suburb)
        {
            JazMax.Web.ViewModel.UserAccountView.CoreBranchView m = new ViewModel.UserAccountView.CoreBranchView();
            m.BranchName = BranchName;
            m.City = City;
            m.CoreTeamLeaderId = Convert.ToInt32(CoreTeamLeaderId);
            m.EmailAddress = EmailAddress;
            m.IsActive = true;
            m.Phone = Phone;

            JazMaxIdentityHelper.UserName = User.Identity.Name;
            if (JazMaxIdentityHelper.IsUserInRole("PA"))
            {
                m.ProvinceId = JazMaxIdentityHelper.GetPAUserInformation(User.Identity.Name).ProvinceId;
            }
            else
            {
                m.ProvinceId = Convert.ToInt32(ProvinceId);
            }
            m.StreetAddress = StreetAddress;
            m.Suburb = Suburb;
            o.CreateNewBranch(m);
            return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(o.Details((int)id));
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(o.Details((int)id));
        }

        public ActionResult UpdateBranch(string BranchId, string CoreTeamLeaderId, string ProvinceId, string BranchName, string Phone, string EmailAddress, string StreetAddress, string City, string Suburb)
        {
            try
            {
                JazMax.Web.ViewModel.UserAccountView.CoreBranchView m = new ViewModel.UserAccountView.CoreBranchView();
                m.BranchId = Convert.ToInt16(BranchId);
                m.BranchName = BranchName;
                m.City = City;
                if (CoreTeamLeaderId == null)
                {

                }
                else
                {
                    m.CoreTeamLeaderId = Convert.ToInt32(CoreTeamLeaderId);
                }
                m.EmailAddress = EmailAddress;
                m.IsActive = true;
                m.Phone = Phone;

                JazMaxIdentityHelper.UserName = User.Identity.Name;
                if (JazMaxIdentityHelper.IsUserInRole("PA"))
                {
                    m.ProvinceId = JazMaxIdentityHelper.GetPAUserInformation(User.Identity.Name).ProvinceId;
                }
                else
                {
                    m.ProvinceId = Convert.ToInt32(ProvinceId);
                }
                m.StreetAddress = StreetAddress;
                m.Suburb = Suburb;
                o.Update(m);
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Branch could not be updated" }, JsonRequestBehavior.AllowGet);
            }
        }

        #region AJAX Helpers
        public ActionResult GetTeamLeaderForProvince(int Id)
        {
            return Json(_helper.GetTeamLeaderForProvince(Id), JsonRequestBehavior.AllowGet);
        }

        [JazMaxIdentity(UserGroup ="PA")]
        public ActionResult GetMyBranches()
        {
            UserInformation a = JazMaxIdentityHelper.GetPAUserInformation(User.Identity.Name);
            return View(o.GetMyBranchs(a.ProvinceId));
        }

        public ActionResult GetBranch(int ID)
        {
            return Json(o.Details(ID), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}