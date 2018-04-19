using JazMax.Core.SystemHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Unauthorised()
        {
            JazMaxIdentityHelper.UserName = User.Identity.Name;
            return View();
        }

        public string test()
        {
            JazMaxIdentityHelper.UserName = User.Identity.Name;
            if(JazMaxIdentityHelper.IsUserInRole("PA"))
            {
                return "Yes";
            }
            return "No";
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Deactive()
        {
            return View();
        }
    }
}