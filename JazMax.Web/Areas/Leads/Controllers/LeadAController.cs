using JazMax.Core.Leads.Lead_Attachments;
using JazMax.Web.ViewModel.Leads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Areas.Leads.Controllers
{
    public class LeadAController : Controller
    {
        #region Object initialiazation
        LeadAttachmentService helper = new LeadAttachmentService();
        #endregion 

        #region Download Target File
        public FileResult DownLoad(int id)
        {
            LeadAttachments file = helper.FindById(id);
            string fileName = file.FileNames;
            byte[] contents = file.FileContent;
            return File(contents, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion
        public ActionResult Index()
        {
            return View(helper.GetAllAttachments());
        }

        #region Create LeadDocument Attachment

        public ActionResult LeadAttachment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LeadAttachment(LeadAttachments attach)
        {
            if (ModelState.IsValid)
            {
                attach.LeadId = (int)TempData["Lead"];
                helper.Attach(attach);
                return RedirectToAction("Details", "Leads", new { id = attach.LeadId, area = "Leads" });
            }
            return View(attach);
        }
        #endregion

        #region Partial Views
        public PartialViewResult Attachment(int id)
        {
            var attach = helper.GetAttachmentsForDoc(id).ToList();
            return PartialView("_LeadAttachments", attach);
        }
        #endregion

        public PartialViewResult AddAttachment()
        {
            LeadAttachments document = new LeadAttachments();
            return PartialView("_AddAttachment", document);
        }
    }
}