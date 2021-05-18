using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.Leads;
using System.Web;

namespace JazMax.Core.Leads.Lead_Attachments
{
    public class LeadAttachmentService
    {


        #region GetAll Attachments
        public IQueryable<DataAccess.CoreLeadAttachment> GetLeadAttachment()
        {
            using (DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var documents = db.CoreLeadAttachments.ToList();
                return documents.AsQueryable();
            }
        }

        public IQueryable<LeadAttachments> GetAllAttachments()
        {
            var model = GetLeadAttachment().Select(x => new LeadAttachments
            {
                FileAttachmentId = x.FileAttachmentId,
                LeadId = x.LeadId,
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
        public LeadAttachments FindById(int id)
        {
            var model = GetAllAttachments().FirstOrDefault(x => x.FileAttachmentId == id);
            return model;
        }
        #endregion

        #region Create Attachments
        public int Attach(LeadAttachments leadAttachments)
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
                        DataAccess.CoreLeadAttachment AttachmentUpload = new DataAccess.CoreLeadAttachment()
                        {
                            FileAttachmentId = leadAttachments.FileAttachmentId,
                            LeadId = leadAttachments.LeadId,
                            FileNames = System.IO.Path.GetFileName(file.FileName),
                            CoreUserId = 1,
                            DateCreated = DateTime.Now,
                            DeletedBy = "None",
                            DeletedDate = DateTime.Now,
                            BranchId = 1,
                            ProvinceId = 1,
                            FileAttachmentDescription = leadAttachments.FileAttachmentDescription,
                            LastUpdated = DateTime.Now,
                            IsActive = true,
                            IsRecieved = true,
                            FileContent = leadAttachments.FileContent
                        };
                        #endregion

                        #region Binary Reader
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            AttachmentUpload.FileContent = reader.ReadBytes(file.ContentLength);
                        }
                        #endregion

                        db.CoreLeadAttachments.Add(AttachmentUpload);
                    }
                    #endregion

                    db.SaveChanges();
                }
                return leadAttachments.FileAttachmentId;
            }
        }
        #endregion

        #region GetAttachments For Lead
        public IQueryable<LeadAttachments> GetAttachmentsForDoc(int id)
        {
            var files = GetAllAttachments().Where(x => x.LeadId == id).ToList();
            return files.AsQueryable();
        }
        #endregion

    }
}
