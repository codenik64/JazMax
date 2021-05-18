using JazMax.Core.Documents;
using JazMax.Core.Documents.DocumentAttachment;
using JazMax.Core.SystemHelpers;
using JazMax.Web.ViewModel.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Controllers
{
    public class AttachmentsController : Controller
    {
        #region Declarations
        AttachmentHelper helper = new AttachmentHelper();
        FileHelper help = new FileHelper();
        #endregion

        #region Download Target File
        public FileResult DownLoad(int id)
        {

            DocumentAttachments file = helper.FindById(id);
            string fileName = file.FileNames;
            byte[] contents = file.FileContent;
            return File(contents, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion

        #region All Attachments
        public ActionResult Index()
        {
            return View(helper.GetAllAttachments());
        }
        #endregion

        #region Create Document Attachment

        public ActionResult Attachments()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Attachments(DocumentAttachments attach)
        {
            if (ModelState.IsValid)
            {
                attach.FileUploadId = (int)TempData["File"];
                helper.Attach(attach);
                return RedirectToAction("Details", "Documents",new { id = attach.FileUploadId});
            }
            return View(attach);
        }
        #endregion

        #region Partial Views
        public PartialViewResult Attachment(int id)
        {
            var attach = helper.GetAttachmentsForDoc(id).ToList();
            return PartialView("_Attachments", attach);
        }

        public PartialViewResult CreateAttachment()
        {
            DocumentAttachments document = new DocumentAttachments();
            return PartialView("_CreateAttachment", document);
        }
        #endregion
    }
}