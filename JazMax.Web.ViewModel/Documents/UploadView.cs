using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JazMax.Web.ViewModel.Documents
{
    public class UploadView
    {
        [Key]
        public int FileUploadId { get; set; }
        public string FileNames { get; set; }
        public int CoreUserId { get; set; }
        public int CoreUserTypeId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string DeletedBy { get; set; }
        public System.DateTime DeletedDate { get; set; }
        public int BranchId { get; set; }
        public int ProvinceId { get; set; }
        public int CoreFileTypeId { get; set; }
        public string FileDescription { get; set; }
        public int CoreFileCategoryId { get; set; }
        public System.DateTime LastUpdated { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsSent { get; set; }
        public Nullable<bool> IsRecieved { get; set; }
        public byte[] FileContent { get; set; }
        public string SentFrom { get; set; }
        public string SentTo { get; set; }

    }
}
