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
        [JazMaxIdentity(UserGroup = "TeamLeader")]
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

        [HttpPost]
        public ActionResult Create(JazMax.Web.ViewModel.UserAccountView.CoreProvinceView model)
        {
            if (ModelState.IsValid)
            {
                model.IsActive = true;
                model.IsAssigned = false;
                obj.Create(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }
#endregion


    }
}