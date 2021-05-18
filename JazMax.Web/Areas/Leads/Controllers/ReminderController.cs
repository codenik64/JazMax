using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Core.Leads.Reminder;
using static JazMax.Core.Leads.Reminder.ReminderLogic;
using JazMax.Core.SystemHelpers;
namespace JazMax.Web.Areas.Leads.Controllers
{
    public class ReminderController : Controller
    {
        // GET: Leads/Reminder
        public ActionResult Index()
        {
            LeadReminderSearch index = new LeadReminderSearch()
            {
                BranchId = 0,
                CoreUserId = 0,
                ProvinceId = 0
            };

            JazMax.Core.SystemHelpers.JazMaxIdentityHelper.UserName = User.Identity.Name;

            if(JazMaxIdentityHelper.IsUserInRole("Agent"))
            {
                index.CoreUserId = JazMaxIdentityHelper.GetCoreUserId();
            }

            if (JazMaxIdentityHelper.IsUserInRole("TeamLeader"))
            {
                index.BranchId = JazMaxIdentityHelper.GetTeamLeadersInfoNew().CoreBranchId;
            }

            if (JazMaxIdentityHelper.IsUserInRole("PA"))
            {
                index.ProvinceId = JazMaxIdentityHelper.GetPAUserInformation(User.Identity.Name).ProvinceId;
            }
            ReminderLogic o = new ReminderLogic();
            return View(o.GetMyLeadReminders(index));
        }
    }
}