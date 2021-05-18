using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Leads
{
    public class LeadAttachments
    {
        [Key]
        public int FileAttachmentId { get; set; } 
        public int LeadId { get; set; } 
        public string FileNames { get; set; } 
        public int CoreUserId { get; set; } 
        public System.DateTime DateCreated { get; set; } 
        public System.DateTime DeletedDate { get; set; }
        public string DeletedBy { get; set; } // DeletedBy
        public int BranchId { get; set; } 
        public int ProvinceId { get; set; } 
        public string FileAttachmentDescription { get; set; } 
        public System.DateTime LastUpdated { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsRecieved { get; set; }
        public byte[] FileContent { get; set; } 
    }
}
