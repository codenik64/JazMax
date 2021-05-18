using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Core.Documents.DocumentType;
using JazMax.Web.ViewModel.Documents;
using System.Data.Entity;
using JazMax.Core.Documents;

namespace JazMax.Web.Controllers
{
    public class DocumentTypeController : Controller
    {
        DocumentHelper helper = new DocumentHelper();
        FileHelper help = new FileHelper();
        // GET: DocumentType
        public ActionResult Index()
        {
            return View(helper.GetAllTypes());
        }


        public ActionResult Browse()
        {
            return View();
        }



        public ActionResult CreateType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateType(DocumentTypesView doctype)
        {
            if (ModelState.IsValid)
            {
                var model = helper.CreateDocument(doctype);
                return RedirectToAction("Index");
            }
            return View(doctype);
        }
    }
}