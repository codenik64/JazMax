using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Core.Leads.Logic;
using JazMax.Web.ViewModel.Leads;
using JazMax.Core.SystemHelpers;
namespace JazMax.Web.Areas.Leads.Controllers
{
    public class LeadsController : Controller
    {
        // GET: Leads/Leads
        public ActionResult Index()
        {
            LeadHelper o = new LeadHelper();
            JazMaxIdentityHelper.UserName = User.Identity.Name;
            // return View(o.GetLeadIndex((int)JazMaxIdentityHelper.GetUserInformationNew().BranchId));
            return View(o.GetLeadIndex((int)JazMaxIdentityHelper.GetUserInformationNew().BranchId));
        }

        #region Search Core (Lead Index)
        public ActionResult Search(string ProspectName, string LeadTypeId, string LeadStatusId, string BranchId, string AngentId)
        {
            JazMaxIdentityHelper.UserName = User.Identity.Name;
            LeadHelper o = new LeadHelper();

            LeadIndexSearch model = new LeadIndexSearch()
            {
                ProspectName = ProspectName,
                AngentId = Convert.ToInt32(AngentId),
                BranchId = Convert.ToInt32(BranchId),
                LeadStatusId = Convert.ToInt32(LeadStatusId),
                LeadTypeId = Convert.ToInt32(LeadTypeId)
            };

            if (JazMaxIdentityHelper.IsUserInRole(JazMax.Common.Enum.UserType.Agent.ToString()))
            {
                model.AngentId = JazMaxIdentityHelper.GetAgentId();
                return View(o.GetLeadIndexNew(model));
            }

            if (JazMaxIdentityHelper.IsUserInRole(JazMax.Common.Enum.UserType.TeamLeader.ToString()))
            {
                model.BranchId = JazMaxIdentityHelper.GetTeamLeadersInfoNew().CoreBranchId;
                return View(o.GetLeadIndexNew(model));
            }

            if (JazMaxIdentityHelper.IsUserInRole(JazMax.Common.Enum.UserType.PA.ToString()))
            {
                model.BranchIdList = JazMaxIdentityHelper.GetPAProvinceIdList();
                return View(o.GetLeadIndexNew(model));
            }

            if (JazMaxIdentityHelper.IsUserInRole(JazMax.Common.Enum.UserType.CEO.ToString()))
            {
                return View(o.GetLeadIndexNew(model));
            }

            return View(o.GetLeadIndexNew(model));
        }
        #endregion

        #region Create Manual Lead
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(JazMax.Core.Leads.Creation.LeadItem model)
        {
            if (ModelState.IsValid)
            {
                model.IsManual = true;
                JazMax.Core.SystemHelpers.JazMaxIdentityHelper.UserName = User.Identity.Name;
                model.CoreUserId = JazMax.Core.SystemHelpers.JazMaxIdentityHelper.GetCoreUserId();
                JazMax.Core.Leads.Creation.LeadCreation.CaptureLead(model);
                return RedirectToAction("Search");
            }
            return View(model);
        }
        #endregion

        #region Lead Details
        public ActionResult Details(int Id)
        {
            LeadHelper o = new LeadHelper(Id);
            return View(o.GetLead());
        }
        #endregion

        #region Lead Dashboard
        public ActionResult LeadDashboard()
        {
            return View();
        }
        #endregion

        #region AJAX PostBacks
        public JsonResult CreateActivity(string LeadActivityId, string LeadId, string Description)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                JazMax.Core.Leads.Activity.LeadActivity model = new Core.Leads.Activity.LeadActivity()
                {
                    CoreUserId = JazMaxIdentityHelper.GetCoreUserId(),
                    Description = Description,
                    IsSystem = false,
                    LeadActivityId = Convert.ToInt32(LeadActivityId),
                    LeadId = Convert.ToInt32(LeadId)
                };

                JazMax.Core.Leads.Activity.ActivityCreation.CaptureLeadActivity(model);

                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Success,
                    Message = Common.Models.JsonMessage.Saved,
                });
            }
            catch
            {
                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Error,
                    Message = Common.Models.JsonMessage.Error,
                });
            }
        }

        public JsonResult CreateReminder(string LeadId, string DescriptionReminder, string Date)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                JazMax.Core.Leads.Reminder.LeadReminder model = new Core.Leads.Reminder.LeadReminder()
                {
                    CoreUserId = JazMaxIdentityHelper.GetCoreUserId(),
                    Description = DescriptionReminder,
                    ReminderDate = Convert.ToDateTime(Date),
                    LeadId = Convert.ToInt32(LeadId)
                };

                JazMax.Core.Leads.Reminder.ReminderCreation.CaptureLeadReminder(model);

                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Success,
                    Message = Common.Models.JsonMessage.Saved,
                });
            }
            catch
            {
                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Error,
                    Message = Common.Models.JsonMessage.Error,
                });
            }
        }

        public JsonResult CloseLead(string LeadId, string DescriptionText)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                JazMax.Core.Leads.Status.LeadStatusLogic.CloseLead(Convert.ToInt32(LeadId), new Core.Leads.Activity.LeadActivity
                {
                    CoreUserId = JazMaxIdentityHelper.GetCoreUserId(),
                    Description = "Lead has been closed by " + JazMaxIdentityHelper.GetBasicUserInfo().DisplayName,
                    IsSystem = false,
                    LeadActivityId = 6, //Lead CLosed
                    LeadId = Convert.ToInt32(LeadId)

                });
                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Success,
                    Message = Common.Models.JsonMessage.Saved,
                });
            }
            catch (Exception e)
            {
                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Error,
                    Message = Common.Models.JsonMessage.Error,
                });
            }
        }

        public JsonResult CreateNewLead(string FullName, string ContactNumber, string Email, string Comments, string ProptyId)
        {
            try
            {
                JazMax.Core.Leads.Creation.LeadItem model = new Core.Leads.Creation.LeadItem()
                {
                    Comments = Comments,
                    ContactNumber = ContactNumber,
                    CoreUserId = null,
                    Email = Email,
                    FullName = FullName,
                    IsManual = false,
                    PropertyListingID = Convert.ToInt32(ProptyId),
                    Source = "JazMax.co.za",
                    
                };

                JazMax.Core.Leads.Creation.LeadCreation.CaptureLead(model);

                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Success,
                    Message = Common.Models.JsonMessage.Saved,
                });
            }
            catch (Exception e)
            {
                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Error,
                    Message = Common.Models.JsonMessage.Error,
                });
            }
        }
        #endregion
    }
}