using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Controllers
{
    public class LogController : Controller
    {

        // GET: Log
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLog(string tableName, int Id)
        {
            return View(JazMax.BusinessLogic.ChangeLog.ChangeLogService.GetEditLog(tableName, Id));
        }
    }
}