using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.Documents;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Core.Documents.DocumentAttachment
{
    public class AttachmentHelper
    {

        FileHelper help = new FileHelper();

        #region GetAll Attachments
        public IQueryable<DataAccess.CoreDocumentAttachment> GetDocumentAttachment()
        {
            using (DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var documents = db.CoreDocumentAttachments.ToList();
                return documents.AsQueryable();
            }
        }

        public IQueryable<DocumentAttachments> GetAllAttachments()
        {
            var model = GetDocumentAttachment().Select(x => new DocumentAttachments
            {
                FileAttachmentId = x.FileAttachmentId,
                FileUploadId = x.FileUploadId,
                FileNames = x.FileNames,
                CoreUserId = x.CoreUserId,
                DateCreated = x.DateCreated,
                DeletedBy = x.DeletedBy,
                DeletedDate = x.DeletedDate,
                BranchId = x.BranchId,
                ProvinceId = x.ProvinceId,
                FileAttachmentDescription = x.FileAttachmentDescription,
                LastUpdated = x.LastUpdated,
                IsActive = x.IsActive,
                IsRecieved = x.IsRecieved,
                FileContent = x.FileContent,

            }).ToList();
            return model.AsQueryable();
        }
        #endregion

        #region FindAttachmentById
        public DocumentAttachments FindById(int id)
        {
            var model = GetAllAttachments().FirstOrDefault(x => x.FileAttachmentId == id);
            return model;
        }
        #endregion

        #region Create Attachments
        public int Attach(DocumentAttachments Attachments)
        {
            using (DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var context = HttpContext.Current;
                for (int i = 0; i < context.Request.Files.Count; i++)
                {
                    var file = context.Request.Files[i];

                    #region Content Length
                    if (file != null && file.ContentLength > 0)
                    {
                        #region AttachmentFileUpload
                        DataAccess.CoreDocumentAttachment AttachmentUpload = new DataAccess.CoreDocumentAttachment()
                        {
                            FileAttachmentId = Attachments.FileAttachmentId,
                            FileUploadId = Attachments.FileUploadId,
                            FileNames = System.IO.Path.GetFileName(file.FileName),
                            CoreUserId = 1,
                            DateCreated = DateTime.Now,
                            DeletedBy = "None",
                            DeletedDate = DateTime.Now,
                            BranchId = 1,
                            ProvinceId = 1,
                            FileAttachmentDescription = Attachments.FileAttachmentDescription,
                            LastUpdated = DateTime.Now,
                            IsActive = true,
                            IsRecieved = true,
                            FileContent = Attachments.FileContent
                        };
                        #endregion

                        #region Binary Reader
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            AttachmentUpload.FileContent = reader.ReadBytes(file.ContentLength);
                        }
                        #endregion

                        db.CoreDocumentAttachments.Add(AttachmentUpload);
                    }
                    #endregion

                    db.SaveChanges();
                }
                return Attachments.FileAttachmentId;
            }
        }
        #endregion

        #region GetAttachments For Document
        public IQueryable<DocumentAttachments> GetAttachmentsForDoc(int id)
        {
            var files = GetAllAttachments().Where(x => x.FileUploadId == id).ToList();
            return files.AsQueryable();
        }
        #endregion






    }
}
