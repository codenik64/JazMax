using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class CoreBranchView
    {
        public int BranchId { get; set; }
        [Display(Name = "Teamleader")]
        public Nullable<int> CoreTeamLeaderId { get; set; }
        [Display(Name = "Province")]
        public Nullable<int> ProvinceId { get; set; }
        [Display(Name = "IsActive")]
        public Nullable<bool> IsActive { get; set; }
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Suburb")]
        public string Suburb { get; set; }
        [Display(Name = "Team Leader")]
        public string TeamLeaderName { get; set; }
        [Display(Name = "Province")]
        public string ProvinceName { get; set; }
    }
}
