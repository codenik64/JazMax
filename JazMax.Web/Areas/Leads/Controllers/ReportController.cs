using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Core.Leads.Reports;
using JazMax.Web.ViewModel.Leads.Reports;
using JazMax.Core.SystemHelpers;
namespace JazMax.Web.Areas.Leads.Controllers
{
    public class ReportController : Controller
    {
 
        #region Leads By Agent
        public ActionResult LeadByAnAgent()
        {
            LeadActivityByAgentFilter model = new LeadActivityByAgentFilter() { ShowReport = false };
            return View(model);
        }

        [HttpPost]
        public ActionResult LeadByAnAgent(LeadActivityByAgentFilter model)
        {
            model.ShowReport = true;
            model.Result = LeadReportCore.LeadByAgentNew(model);
            return View(model);
        }
        #endregion

        #region Leads By Branch
        public ActionResult LeadByABranch()
        {
            LeadActivityByBranchFilter model = new LeadActivityByBranchFilter() { ShowReport = false };
            return View(model);
        }

        [HttpPost]
        public ActionResult LeadByABranch(LeadActivityByBranchFilter model)
        {
            model.ShowReport = true;
            model.Results = LeadReportCore.LeadByBranch(model);
            return View(model);
        }
        #endregion

        #region Leads Closed
        public ActionResult LeadsClosed()
        {
            DateTime dCalcDate = DateTime.Now;
            LeadClosedReportFilter model = new LeadClosedReportFilter()
            {
                ShowReport = false,
                DateFrom = new DateTime(dCalcDate.Year, dCalcDate.Month, 1),
                DateTo = new DateTime(dCalcDate.Year, dCalcDate.Month, DateTime.DaysInMonth(dCalcDate.Year, dCalcDate.Month))
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult LeadsClosed(LeadClosedReportFilter model)
        {
            model.LeadStatusId = 3; //Lead Closed
            model.ShowReport = true;
            model.Result = LeadReportCore.LeadClosedReport(model);
            return View(model);
        }
        #endregion

        #region Leads By Property
        public ActionResult LeadsByProperty()
        {
            DateTime dCalcDate = DateTime.Now;
            LeadsByPropertyFilter model = new LeadsByPropertyFilter()
            {
                ShowReport = false,
                DateFrom = new DateTime(dCalcDate.Year, dCalcDate.Month, 1),
                DateTo = new DateTime(dCalcDate.Year, dCalcDate.Month, DateTime.DaysInMonth(dCalcDate.Year, dCalcDate.Month))
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult LeadsByProperty(LeadsByPropertyFilter model)
        {
            model.ShowReport = true;
            model.Result = LeadReportCore.LeadsByProperty(model);
            return View(model);
        }
        #endregion
    }
}