using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Controllers
{
    public class PAController : Core.SystemHelpers.JazMaxControllerCore
    {
        // GET: PA
        public ActionResult Index()
        {
            return JazMaxView();
        }

        public PartialViewResult CreateAgent()
        {
            return PartialView();
        }
    }
}