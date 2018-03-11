using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.BusinessLogic.UserAccounts;

namespace JazMax.Web.Controllers
{
    public class ProvinceController : Controller
    {
        private static CoreProvinceService obj = new CoreProvinceService();
        // GET: Province
        public ActionResult Index()
        {
            return View(obj.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(JazMax.Web.ViewModel.UserAccountView.CoreProvinceView model)
        {
            if(ModelState.IsValid)
            {
                model.IsActive = true;
                model.IsAssigned = false;
                obj.Create(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }


    }
}