using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Web.ViewModel.PropertyManagement.CaptureListing;
using JazMax.Core.Property.PropertyManagement;

namespace JazMax.Web.Areas.Property.Controllers
{
    public class PropertyListingController : Controller
    {
        private static PropertyListingCoreService o = new PropertyListingCoreService();
        // GET: Property/PropertyListing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NewListingView model)
        {
            try
            {
                o.MainInsert(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
    }
}