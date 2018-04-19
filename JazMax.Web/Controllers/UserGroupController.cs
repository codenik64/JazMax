using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.BusinessLogic.UserAccounts;

namespace JazMax.Web.Controllers
{
    public class UserGroupController : Controller
    {
        private static UserGroupService obj = new UserGroupService();

        #region Get All
        public ActionResult Index()
        {
            return View(obj.GetAll());
        }
        #endregion

        #region Create User Group
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(JazMax.Web.ViewModel.UserAccountView.CoreUserTypeView model)
        {
            if (ModelState.IsValid)
            {
                model.IsActive = true;
                obj.CreateUserGroup(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }
#endregion
    }
}