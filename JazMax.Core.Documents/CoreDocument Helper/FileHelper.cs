using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.Documents;
using JazMax.Core.SystemHelpers;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Core.Documents
{
    public class FileHelper
    {
        #region GetAllDocuments
        public IQueryable<DataAccess.CoreFileUpload> GetDocument()
        {
            using (DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var documents = db.CoreFileUploads.ToList();
                 return documents.AsQueryable();
            }
        }

        public IQueryable<UploadView> GetAllDocuments()
        {
            var documents = GetDocument().Select(x => new UploadView
            {
                FileUploadId = x.FileUploadId,
                FileNames = x.FileNames,
                CoreUserId = x.CoreUserId,
                CoreUserTypeId = x.CoreUserTypeId,
                DateCreated = x.DateCreated,
                DeletedBy = x.DeletedBy,
                DeletedDate = x.DeletedDate,
                BranchId = x.BranchId,
                ProvinceId = x.ProvinceId,
                CoreFileTypeId = x.CoreFileTypeId,
                FileDescription = x.FileDescription,
                CoreFileCategoryId = x.CoreFileCategoryId,
                LastUpdated = x.LastUpdated,
                IsActive = (bool)x.IsActive,
                IsSent = (bool)x.IsSent,
                IsRecieved = (bool)x.IsRecieved,
                FileContent = x.FileContent,
                SentFrom = x.SentFrom,
                SentTo = x.SentTo,

            }).ToList();

            return documents.AsQueryable();
        }
        #endregion

        #region FindById
        public UploadView FindById(int id)
        {
            return GetAllDocuments().FirstOrDefault(x => x.FileUploadId == id);
           
        }
        #endregion

        #region UploadFile to Document Directory
        public int Upload(UploadView coreFileUpload)
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
                        #region CoreFIleUpload
                        DataAccess.CoreFileUpload fileUpload = new DataAccess.CoreFileUpload()
                        {
                            FileUploadId = coreFileUpload.FileUploadId,
                            FileNames = System.IO.Path.GetFileName(file.FileName),
                            CoreUserId = JazMaxIdentityHelper.GetCoreUserId(),
                            CoreUserTypeId = JazMaxIdentityHelper.GetCoreUserId(),
                            DateCreated = DateTime.Now,
                            DeletedBy = "None",
                            DeletedDate = DateTime.Now,
                            BranchId = (int)JazMaxIdentityHelper.GetUserInformationNew().BranchId,
                            ProvinceId = (int)JazMaxIdentityHelper.GetUserInformationNew().ProvinceId,
                            CoreFileTypeId = coreFileUpload.CoreFileTypeId,
                            FileDescription = coreFileUpload.FileDescription,
                            CoreFileCategoryId = coreFileUpload.CoreFileCategoryId,
                            LastUpdated = DateTime.Now,
                            IsActive = true,
                            IsSent = true,
                            IsRecieved = true,
                            FileContent = coreFileUpload.FileContent,
                            SentFrom = JazMaxIdentityHelper.UserName,
                            SentTo = "Document Directory"
                        };
                        #endregion

                        #region Binary Reader
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            fileUpload.FileContent = reader.ReadBytes(file.ContentLength);
                        }
                        #endregion

                        db.CoreFileUploads.Add(fileUpload);
                    }
                    #endregion
                    db.SaveChanges();
                }
                return coreFileUpload.FileUploadId;
            }
        }

        #endregion

        #region UploadFile To User
        public int UploadToUser(UploadView coreFileUpload)
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
                        #region CoreFIleUpload
                        DataAccess.CoreFileUpload fileUpload = new DataAccess.CoreFileUpload()
                        {
                            FileUploadId = coreFileUpload.FileUploadId,
                            FileNames = System.IO.Path.GetFileName(file.FileName),
                            CoreUserId = JazMaxIdentityHelper.GetCoreUserId(),
                            CoreUserTypeId = JazMaxIdentityHelper.GetCoreUserId(),
                            DateCreated = DateTime.Now,
                            DeletedBy = "None",
                            DeletedDate = DateTime.Now,
                            BranchId = (int)JazMaxIdentityHelper.GetUserInformationNew().BranchId,
                            ProvinceId = (int)JazMaxIdentityHelper.GetUserInformationNew().ProvinceId,
                            CoreFileTypeId = coreFileUpload.CoreFileTypeId,
                            FileDescription = coreFileUpload.FileDescription,
                            CoreFileCategoryId = coreFileUpload.CoreFileCategoryId,
                            LastUpdated = DateTime.Now,
                            IsActive = true,
                            IsSent = true,
                            IsRecieved = false,
                            FileContent = coreFileUpload.FileContent,
                            SentFrom = JazMaxIdentityHelper.UserName,
                            SentTo = coreFileUpload.SentTo
                        };
                        #endregion

                        #region Binary Reader
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            fileUpload.FileContent = reader.ReadBytes(file.ContentLength);
                        }
                        #endregion

                        db.CoreFileUploads.Add(fileUpload);
                    }
                    #endregion
                    db.SaveChanges();
                }
                return coreFileUpload.FileUploadId;
            }
        }

        #endregion

        #region GetAll Documents For Core User
        public IQueryable<UploadView> GetDocumentsForUser(int id)
        {
            var files = GetAllDocuments().Where(x => x.CoreUserId == id).ToList();
            return files.AsQueryable();
        }
        #endregion

        #region GetAll Documents For Core User
        public IQueryable<UploadView> GetDocumentsUser()
        {
            var files = GetAllDocuments().Where(x => x.SentFrom == HttpContext.Current.User.Identity.Name && x.SentTo == "Document Directory").ToList();
            return files.AsQueryable();
        }
        #endregion

        #region GetAll Personal Recieved Documents
        public IQueryable<UploadView> GetAllPersonal()
        {
            return GetAllDocuments().Where(x => x.SentTo == HttpContext.Current.User.Identity.Name);
        }
        #endregion

        #region GetAll Personal Sent Documents
        public IQueryable<UploadView> GetAllSent()
        {
            return GetAllDocuments().Where(x => x.SentFrom == HttpContext.Current.User.Identity.Name);
        }
        #endregion

        #region GetAll Pending Incoming Documents
        public IQueryable<UploadView> GetAllIncoming()
        {
            var incoming = GetAllDocuments().Where(x => x.IsRecieved == false).ToList();
            return incoming.AsQueryable();
        }
        #endregion

        #region GetAll Processed Documents
        public IQueryable<UploadView> GetAllProcessedDocuments()
        {
            var Processed = GetAllDocuments().Where(x => x.IsRecieved == true).ToList();
            return Processed.AsQueryable();
        }
        #endregion

        #region GetDocuments In a Branch

        #endregion

        #region GetDocuments In a Province

        #endregion

        #region Forward Document To user
        public int ForwardDocument(int id, UploadView coreFileUpload , FormCollection Form)
        {
            using (DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                string[] ids = Form["SentTo"].Split(new char[] { ',' });
                //Find Document Id That i Want to forward
                var FindContent = FindById(id);
                //get All contents of that particular file
                //Move Document to User Email Thats been Selected
                #region Database Upload object
                foreach (var item in ids)
                {
                    DataAccess.CoreFileUpload upload = new DataAccess.CoreFileUpload()
                    {
                        FileUploadId = FindContent.FileUploadId,
                        FileNames = FindContent.FileNames,
                        CoreUserId = JazMaxIdentityHelper.GetCoreUserId(),
                        CoreUserTypeId = FindContent.CoreUserTypeId,
                        DateCreated = DateTime.Now,
                        DeletedBy = "None",
                        DeletedDate = DateTime.Now,
                        BranchId = (int)JazMaxIdentityHelper.GetUserInformationNew().BranchId,
                        ProvinceId = (int)JazMaxIdentityHelper.GetUserInformationNew().ProvinceId,
                        CoreFileTypeId = FindContent.CoreFileTypeId,
                        FileDescription = coreFileUpload.FileDescription,
                        CoreFileCategoryId = FindContent.CoreFileCategoryId,
                        LastUpdated = DateTime.Now,
                        IsActive = true,
                        IsSent = true,
                        IsRecieved = false,
                        FileContent = FindContent.FileContent,
                        SentFrom = JazMaxIdentityHelper.UserName,
                        SentTo = item.ToString()
                    };

                    #endregion

                    //SaveChanges To Database
                    db.CoreFileUploads.Add(upload);
                    db.SaveChanges();
                }
                    return FindContent.FileUploadId;
                
            }
        }
        #endregion

        #region GetAll Archived Documents
        public IQueryable<UploadView>GetAllArchived()
        {
            var archive = GetAllDocuments().Where(x => x.IsActive == false).ToList();
            return archive.AsQueryable();
        }
        #endregion

        #region Archive Documents
        public void ArchiveDocument(UploadView upload)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
            {
                DataAccess.CoreFileUpload UploadDocument = new DataAccess.CoreFileUpload();
                UploadDocument = db.CoreFileUploads.FirstOrDefault(x => x.FileUploadId == upload.FileUploadId);
                UploadDocument.IsActive = false;
                db.SaveChanges();
            }

        }
        #endregion

        #region Bulk Archive
        public void BulkArchive(FormCollection Form)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
            {
                string[] ids = Form["FileUploadId"].Split(new char[] { ',' });
                foreach (var item in ids)
                {
                    var file = db.CoreFileUploads.Find(int.Parse(item));
                    file.IsActive = false;
                    db.SaveChanges();
                }
               
            }
        }
        #endregion

        #region Bulk Recovery
        public void BulkRecover(FormCollection Form)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
            {
                string[] ids = Form["FileUploadId"].Split(new char[] { ',' });
                foreach (var item in ids)
                {
                    var file = db.CoreFileUploads.Find(int.Parse(item));
                    file.IsActive = true;
                    db.SaveChanges();
                }

            }
        }
        #endregion

        #region Bulk Delete
        public void BulkRemove(FormCollection Form)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
            {
                string[] ids = Form["FileUploadId"].Split(new char[] { ',' });
                foreach (var item in ids)
                {
                    var file = db.CoreFileUploads.Find(int.Parse(item));
                    db.CoreFileUploads.Remove(file);
                    db.SaveChanges();
                }

            }
        }
        #endregion

        #region Recover Documents
        public void RecoverDocument(UploadView upload)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
            {
                DataAccess.CoreFileUpload UploadDocument = new DataAccess.CoreFileUpload();
                UploadDocument = db.CoreFileUploads.FirstOrDefault(x => x.FileUploadId == upload.FileUploadId);
                UploadDocument.IsActive = true;
                db.SaveChanges();
            }

        }
        #endregion

        #region Update IsRecieved If User Has Recieved the Documents
        public void RecievedDocuments(UploadView upload)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new JazMax.DataAccess.JazMaxDBProdContext())
            {
                DataAccess.CoreFileUpload UploadDocument = new DataAccess.CoreFileUpload();
                UploadDocument = db.CoreFileUploads.FirstOrDefault(x => x.FileUploadId == upload.FileUploadId);
                UploadDocument.IsRecieved = true;
                db.SaveChanges();
            }
        }
        #endregion


    }
}
