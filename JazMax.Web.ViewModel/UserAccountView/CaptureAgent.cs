using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class CaptureAgent
    {
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
    }
}
