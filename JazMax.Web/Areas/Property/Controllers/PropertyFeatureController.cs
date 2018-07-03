using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Core.Property.PropertyManagement;
using JazMax.Web.ViewModel.PropertyManagement;
using System.Threading;

namespace JazMax.Web.Areas.Property.Controllers
{
    public class PropertyFeatureController : Controller
    {
        private static PropertyFeatureService o = new PropertyFeatureService();

        public ActionResult Index()
        {
            return View(o.GetAll(true));
        }

        public JsonResult CreateFeature(string txtFeature)
        {
            try
            {
                PropertyFeatureView model = new PropertyFeatureView()
                {
                    FeatureName = txtFeature
                };
                o.Create(model);
                return Json(new { Result = "Success", Message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = "Error!", Message = "Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return View(o.GetById((int)Id));
        }
    }
}