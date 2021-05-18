using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Documents
{
    public class DocumentTypesView
    {
        [Key]
        public int CoreFileCategoryId { get; set; } 
        public string CategoryName { get; set; } 
        public Nullable<bool> IsActive { get; set; }
        public List<UploadView> Files { get; set; }
    }
}
