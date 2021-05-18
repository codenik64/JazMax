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
using cc;

namespace JazMax.Core.Blob
{
    public static class BlobStorageService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();
        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

        public static int UploadToBlob(string BlobType, string FileType, HttpPostedFileBase file)
        {
            int blobId = 0;
            try
            {
                var container = GetBlobContainer(BlobType);

                if (container != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var blockBlob = container.GetBlockBlobReference(fileName);
                    blockBlob.UploadFromStream(file.InputStream);
                    return SaveImage(blockBlob.Uri.AbsoluteUri, BlobType, FileType, file.FileName, file.ContentType, file.ContentLength);
                }

                blobId = -1;
            }
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 1);
            }
            return blobId;
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
            catch (Exception e)
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
            catch (Exception e)
            {
                JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(e, 1);
                return null;
            }
        }
    }
}
