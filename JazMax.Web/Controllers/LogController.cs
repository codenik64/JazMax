using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Controllers
{
    public class LogController : JazMax.Core.SystemHelpers.JazMaxControllerCore
    {

        // GET: Log
        //public ActionResult Index()
        //{
        //   // return GetAuditLog();
        //}

        public ActionResult GetLog(string tableName, int Id)
        {
            return View(JazMax.BusinessLogic.ChangeLog.ChangeLogService.GetEditLog(tableName, Id));
        }
    }
}