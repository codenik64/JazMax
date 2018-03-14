using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.BusinessLogic.UserAccounts;
using JazMax.Core.SystemHelpers;

namespace JazMax.Web.Controllers
{
    public class BranchController : Controller
    {
        private static CoreBranchService o = new CoreBranchService();
        private static JazMaxIdentityHelper _helper = new JazMaxIdentityHelper();

        // GET: Branch
        public ActionResult Index()
        {
            return View(o.GetAll());
        }

        public string GetBranch()
        {
            return JazMaxCoreViewEngine.RenderPartialToString("_List", o.GetAll());
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
            m.ProvinceId = Convert.ToInt32(ProvinceId);
            m.StreetAddress = StreetAddress;
            m.Suburb = Suburb;
            o.CreateNewBranch(m);
            return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetTeamLeaderForProvince(int Id)
        {
            return Json(_helper.GetTeamLeaderForProvince(Id), JsonRequestBehavior.AllowGet);
        }
    }
}