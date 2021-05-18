using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Core.Documents;
using JazMax.Web.ViewModel.Documents;

namespace JazMax.Web.Controllers
{
    public class DocumentsController : Controller
    {
        #region declarations
        FileHelper helper = new FileHelper();
        #endregion

        #region Document Directory
        public ActionResult Index()
        {
            var docs = helper.GetAllDocuments().Where(x => x.SentTo == "Document Directory" && x.IsActive == true).OrderByDescending(x => x.DateCreated);
            return View(docs);
        }

        [HttpPost]
        public ActionResult Index(FormCollection Form)
        {
            helper.BulkArchive(Form);
            return RedirectToAction("Index");
        }
        #endregion

        #region Get Documents in Recycle Bin
        public ActionResult ArchivedDocuments()
        {
            var docs = helper.GetAllDocuments().Where(x => x.SentTo == "Document Directory" && x.IsActive == false);
            return View(docs);
        }

        [HttpPost]
        public ActionResult ArchivedDocuments(FormCollection Form)
        {
            helper.BulkRecover(Form);
            return RedirectToAction("Index");
        }
        #endregion

        #region Bulk Remove
        [HttpPost]
        public ActionResult Remove(FormCollection Form)
        {
            helper.BulkRemove(Form);
            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        public ActionResult Details(int id)
        {
            var model = helper.FindById(id);
            return View(model);
        }
        #endregion

        #region Archive Action
        public ActionResult Archive(int id)
        {
            return View(helper.FindById(id));
        }

        [HttpPost]
        public ActionResult Archive(UploadView upload)
        {
            helper.ArchiveDocument(upload);
            return RedirectToAction("Index");
        }
        #endregion

        #region Mark as Recieved
        public ActionResult MarkAsRead(int id)
        {
            return View(helper.FindById(id));
        }

        [HttpPost]
        public ActionResult MarkAsRead(UploadView upload)
        {
            helper.RecievedDocuments(upload);
            return RedirectToAction("IncomingDocuments");
        }
        #endregion

        #region Recover Action

        public ActionResult Recover(int id)
        {
            return View(helper.FindById(id));
        }
        [HttpPost]
        public ActionResult Recover(UploadView upload)
        {
            helper.RecoverDocument(upload);
            return RedirectToAction("Index");
        }
        #endregion

        #region Personal Recieved Documents
        public ActionResult PersonalDocuments()
        {
            var personal = helper.GetAllPersonal().Where(x => x.IsActive == true && x.IsRecieved == true);
            return View(personal);
        }
        #endregion

        #region Personal Sent Documents
        public ActionResult PersonalSentDocuments()
        {
            var personalSent = helper.GetAllSent().Where(x => x.SentTo != "Document Directory");
            return View(personalSent);
        }
        #endregion

        #region Incoming Documents For Today
        public ActionResult IncomingDocuments()
        {
            var Incoming = helper.GetAllIncoming().Where(x => x.IsActive == true && x.SentTo == User.Identity.Name);
            return View(Incoming);
        }
        #endregion

        #region Branch Documents
        public ActionResult MyBranchDocuments()
        {
            return View();
        }
        #endregion

        #region Upload For Document Directory
        public ActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Create(UploadView coreFileUpload)
        {
            if (ModelState.IsValid)
            {
                helper.Upload(coreFileUpload);
                return RedirectToAction("Index");
            }
           
            return View(coreFileUpload);
        }
        #endregion

        #region Upload For Specific user
        public ActionResult CreateForUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateForUser(UploadView coreFileUpload)
        {
            if (ModelState.IsValid)
            {
                helper.UploadToUser(coreFileUpload);
                return RedirectToAction("Index");
            }

            return View(coreFileUpload);
        }
        #endregion

        #region Download Target File
        public FileResult DownLoad(int id)
        {
           
            UploadView file = helper.FindById(id);
            string fileName = file.FileNames;
            string fileType = file.CoreFileTypeId.ToString();
            byte[] contents = file.FileContent;
            return File(contents, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion

        public PartialViewResult GetDocByCat(int id)
        {
            var files = helper.GetAllDocuments().Where(x => x.CoreFileCategoryId == id);
            return PartialView("_UserDocs", files);
        }

        #region Forward Document Action

        public ActionResult Forward()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forward(int id , UploadView coreFileUpload , FormCollection Form)
        {
            
            if (ModelState.IsValid)
            {
                helper.ForwardDocument(id, coreFileUpload , Form);
                return RedirectToAction("Index");
            }
            return View(coreFileUpload);
        }
        #endregion

        #region Partial Views
        public PartialViewResult GetDocs(int id)
        {
            var files = helper.GetDocumentsForUser(id).Where(x => x.SentTo == "Document Directory");
            return PartialView("_UserDocuments", files);
        }
        #endregion
    }
}