using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Core.Documents.FileType;
using JazMax.Web.ViewModel.Documents;
namespace JazMax.Web.Controllers
{
    public class FileTypeController : Controller
    {
        // GET: FileType
        FileTypeHelper helper = new FileTypeHelper();
        public ActionResult Index()
        {
            return View(helper.GetAllFileTypes());
        }

        public ActionResult CreateFileType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFileType(CoreFileTypesView filetype)
        {
            if (ModelState.IsValid)
            {
                var model = helper.CreateFileType(filetype);
                return RedirectToAction("Index");
            }
            return View(filetype);
        }
    }
}