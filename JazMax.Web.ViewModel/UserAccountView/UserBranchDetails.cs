using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class UserBranchDetails
    {
        public int BranchId { get; set; }
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
        [Display(Name = "Teamleader")]
        public string TeamLeader { get; set; }
        public string Province { get; set; }
        public bool HasResult { get; set; }
    }
}
