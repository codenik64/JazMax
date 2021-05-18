using JazMax.Core.Documents.AdditionalNotes;
using JazMax.Web.ViewModel.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Controllers
{
    public class NotesController : Controller
    {
        #region Declarations
        AdditionalNotesHelper helper = new AdditionalNotesHelper();
        #endregion

        #region Partial Views
        public PartialViewResult Note(int id)
        {
            var attach = helper.GetNotesForDoc(id).ToList();
            return PartialView("_NoteList", attach);
        }

        public PartialViewResult CreateNote()
        {
            AdditionalNotesView note = new AdditionalNotesView();
            return PartialView("_AddNote", note);
        }
        #endregion

        public ActionResult Index()
        {
            var model = helper.GetAllNotes();
            return View(model);
        }

        public ActionResult AddNote()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNote(AdditionalNotesView Note)
        {
            if (ModelState.IsValid)
            {
                Note.FileUploadId = (int)TempData["File"];
                helper.AddNote(Note);
                 return RedirectToAction("Details", "Documents", new { id = Note.FileUploadId });
                //return RedirectToAction("Index");
            }
            return View(Note);
        }
    }
}