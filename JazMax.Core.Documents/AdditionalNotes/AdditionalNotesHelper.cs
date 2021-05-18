using JazMax.Web.ViewModel.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.DataAccess;

namespace JazMax.Core.Documents.AdditionalNotes
{
    public class AdditionalNotesHelper
    {
        #region Get All Notes For Document

        #region Get Notes From Database
        public IQueryable<CoreAdditionalNote> GetNotes()
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var query = db.CoreAdditionalNotes.ToList();
                return query.AsQueryable();
            }
          
        }
        #endregion

        #region Populate ViewModel
        public IQueryable<AdditionalNotesView> GetAllNotes()
        {
            var notes = GetNotes().Select(x => new AdditionalNotesView
            {
                AdditionalNoteId = x.AdditionalNoteId,
                FileUploadId = x.FileUploadId,
                CoreUserId = x.CoreUserId,
                DateCreated = x.DateCreated,
                DeletedBy = x.DeletedBy,
                DeletedDate = x.DeletedDate,
                IsActive = x.IsActive,
                IsSent = x.IsSent,
                NotesArea = x.NotesArea
            }).ToList();

            return notes.AsQueryable();
        }
        #endregion

        #endregion

        #region Find Note By Id
        public AdditionalNotesView FindById(int id)
        {
            return GetAllNotes().FirstOrDefault(x => x.AdditionalNoteId == id);
        }
        #endregion

        #region Add Note Helper
        public int AddNote(AdditionalNotesView Note)
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                DataAccess.CoreAdditionalNote AdditionalNotes = new DataAccess.CoreAdditionalNote()
                {
                    AdditionalNoteId = Note.AdditionalNoteId,
                    FileUploadId = Note.FileUploadId,
                    CoreUserId = 1,
                    DateCreated = DateTime.Now,
                    DeletedBy = "None",
                    DeletedDate = DateTime.Now,
                    IsActive = true,
                    IsSent = true,
                    NotesArea = Note.NotesArea
                };
                db.CoreAdditionalNotes.Add(AdditionalNotes);
                db.SaveChanges();
                return AdditionalNotes.AdditionalNoteId;
            }
        }
        #endregion

        #region Get Notes For Document
        public IQueryable<AdditionalNotesView> GetNotesForDoc(int id)
        {
            var files = GetAllNotes().Where(x => x.FileUploadId == id).ToList();
            return files.AsQueryable();
        }
        #endregion
    }
}
