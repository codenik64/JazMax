using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.BusinessLogic.UserAccounts;
using JazMax.Web.ViewModel.UserAccountView;
using JazMax.Core.SystemHelpers;

namespace JazMax.Web.Controllers
{
    public class AgentController : Controller
    {
        private static AgentService o = new AgentService();
        // GET: Agent
        public ActionResult Index()
        {
            return View(o.GetAll());
        }

        public ActionResult MyAgent()
        {
            int teamLeaderId = 0;
            try
            {
                teamLeaderId = JazMaxIdentityHelper.GetTeamLeadersInfoNew(User.Identity.Name).CoreTeamLeaderId;
                return View(o.GetMyAgents(teamLeaderId));
            }
            catch
            {
                teamLeaderId = 0;
            }
            return RedirectToAction("Index");
        }
    }
}