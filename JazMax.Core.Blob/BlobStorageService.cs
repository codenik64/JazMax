using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JazMax.Core.Blob.Model;
using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace JazMax.Core.Blob
{
    public static class BlobStorageService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();
        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

        public static void UploadToBlob (string BlobType, string FileType, HttpPostedFileBase file)
        {
            try
            {
                var container = GetBlobContainer(BlobType);

                if (container != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var blockBlob = container.GetBlockBlobReference(fileName);
                    blockBlob.UploadFromStream(file.InputStream);
                    SaveImage(blockBlob.Uri.AbsolutePath, BlobType, FileType, file.FileName, file.ContentType, file.ContentLength);
                }
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 1);
            }
        }

        private static int SaveImage(string url, string type, string fileType, string fileName, string fileExtension, int fileSize)
        {
            int BlobStorageId = 0;
            try
            {
                DataAccess.BlobCoreStorage blob = new DataAccess.BlobCoreStorage()
                {
                    BlobFileName = fileName,
                    BlobFileType = fileType,
                    BlobType = type,
                    IsActive = true,
                    BlobUrl = url,
                    BlobFileExtension = fileExtension,
                    BlobFileSize = fileSize
                };
                db.BlobCoreStorages.Add(blob);
                db.SaveChanges();
                BlobStorageId = blob.BlobId;
                return BlobStorageId;
            }
            catch(Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 1);
                return BlobStorageId;
            }
        }

        private static CloudBlobContainer GetBlobContainer(string containername)
        {
            try
            {
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(containername);

                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };

                container.CreateIfNotExists();
                container.SetPermissions(permissions);
                return container;
            }
            catch(Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 1);
                return null;
            }
        }
    }
}
