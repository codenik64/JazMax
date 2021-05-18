using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Documents
{
    public class CoreFileTypesView
    {
        [Key]
        public int CoreFileTypeId { get; set; }
        public string TypeName { get; set; } 
        public Nullable<bool> IsActive { get; set; } 
    }
}
