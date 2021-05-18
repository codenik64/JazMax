using JazMax.Core.Property.PropertyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.PropertyWebsite.Controllers
{
    public class PropertyListingController : Controller
    {
        private static PropertyListingCoreService o = new PropertyListingCoreService();
        // GET: PropertyListing
        #region Index View
        public ActionResult Index()
        {
            return View(o.GetPrimaryListingOK());
        }
        #endregion

        #region Details
        public ActionResult PropertyDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = o.FindPrimaryById((int)id);
            return View(query);
        }
        #endregion
    }
}