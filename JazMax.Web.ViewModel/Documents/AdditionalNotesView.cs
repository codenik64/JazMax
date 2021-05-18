using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Documents
{
    public class AdditionalNotesView
    {
        [Key]
        public int AdditionalNoteId { get; set; }
        [Display(Name = "User")]
        public int CoreUserId { get; set; } 
        public System.DateTime DateCreated { get; set; } 
        public string DeletedBy { get; set; } 
        public System.DateTime DeletedDate { get; set; } 
        public int FileUploadId { get; set; } 
        [Display(Name ="Notes")]
        public string NotesArea { get; set; } 
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsSent { get; set; } 
    }
}
