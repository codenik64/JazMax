using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.BusinessLogic.UserAccounts;
using JazMax.Web.Models;
using JazMax.Core.SystemHelpers;
namespace JazMax.Web.Controllers
{
    public class ProvinceController : Controller
    {
        private static CoreProvinceService obj = new CoreProvinceService();
  
        #region Get All 
        [JazMaxIdentity(UserGroup = "TeamLeader,CEO")]
        public ActionResult Index()
        {
            return View(obj.GetAll());
        }
        #endregion

        #region Create Province
        public ActionResult Create()
        {
            return View();
        }

        public JsonResult CreateValue (string txtProvince)
        {
            try
            {
                JazMax.Web.ViewModel.UserAccountView.CoreProvinceView model = new ViewModel.UserAccountView.CoreProvinceView();
                model.ProvinceName = txtProvince;
                model.IsActive = true;
                model.IsAssigned = false;
                obj.Create(model);
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Error, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(CoreProvinceService.GetProvinceDetails((int)id));
        }


        public JsonResult UpdateProvince(string ProvinceName, string ProvinceId)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                CoreProvinceService.Update(ProvinceName, JazMaxIdentityHelper.GetCoreUserId(), Convert.ToInt32(ProvinceId));
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Error, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Activate(string ProvinceId)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                CoreProvinceService.DeactiveCoreProvince(Convert.ToInt32(ProvinceId), JazMaxIdentityHelper.GetCoreUserId(), false);
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Error, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult Deactivate(string ProvinceId)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                CoreProvinceService.DeactiveCoreProvince(Convert.ToInt32(ProvinceId), JazMaxIdentityHelper.GetCoreUserId(), true);
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Error, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}