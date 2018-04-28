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

        #region All Agents
        [JazMaxIdentity(UserGroup = "CEO,Admin")]
        public ActionResult Index()
        {
            return View(o.GetAll());
        }
        #endregion

        #region Teamleader and PA View of Agents Under Them / In Province
        [JazMaxIdentity(UserGroup = "PA,TeamLeader")]
        public ActionResult MyAgent()
        {
            int teamLeaderId = 0;
            JazMaxIdentityHelper.UserName = User.Identity.Name;
                
            try
            {
                if (JazMaxIdentityHelper.IsUserInRole("TeamLeader"))
                {
                    teamLeaderId = JazMaxIdentityHelper.GetTeamLeadersInfoNew().CoreTeamLeaderId;
                    return View(o.GetMyAgents(teamLeaderId));
                }
                else if (JazMaxIdentityHelper.IsUserInRole("PA"))
                {
                    var provinceId = JazMaxIdentityHelper.GetPAUserInformation(User.Identity.Name).ProvinceId;
                    return View(o.GetMyAgentInProvince(provinceId));
                }
            }
            catch (Exception e)
            {
                //JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 0);
                teamLeaderId = 0;
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}