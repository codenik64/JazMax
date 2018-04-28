using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Areas.Leads.Controllers
{
    public class LeadsController : Controller
    {
        // GET: Leads/Leads
        public ActionResult Index()
        {
            return View();
        }
    }
}