using JazMax.Core.SystemHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.PropertyWebsite.Controllers
{
    public class LeadsController : Controller
    {
        // GET: Leads
        public ActionResult Index()
        {
            return View();
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
    }
}