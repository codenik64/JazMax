using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Areas.Property.Controllers
{
    public class PropertyListingController : Controller
    {
        // GET: Property/PropertyListing
        public ActionResult Index()
        {
            return View();
        }
    }
}