using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Core.Blob.Model;
using System.Web;

namespace JazMax.Core.Blob
{
    public interface IBlobStorageService
    {
        int UploadToBlob(Enum type, Enum a, HttpPostedFileBase file);
    }
}
