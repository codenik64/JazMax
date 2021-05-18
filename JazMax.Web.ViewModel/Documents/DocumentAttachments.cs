using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Documents
{
    public class DocumentAttachments
    {
        [Key]
        public int FileAttachmentId { get; set; } 
        public int FileUploadId { get; set; } 
        public string FileNames { get; set; } 
        public int CoreUserId { get; set; } 
        public System.DateTime DateCreated { get; set; } 
        public string DeletedBy { get; set; } 
        public System.DateTime DeletedDate { get; set; }
        public int BranchId { get; set; } 
        public int ProvinceId { get; set; }
        [Display(Name = "Description")]
        public string FileAttachmentDescription { get; set; }
        public System.DateTime LastUpdated { get; set; }
        public Nullable<bool> IsActive { get; set; } 
        public Nullable<bool> IsRecieved { get; set; } 
        public byte[] FileContent { get; set; } 
      
    }
}
