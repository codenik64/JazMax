using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class AgentDetailsView
    {
        public int CoreUserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Cell Number")]
        public string CellPhone { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        [Display(Name = "Teamleader")]
        public Nullable<int> CoreTeamLeaderId { get; set; }
        [Display(Name = "Province")]
        public Nullable<int> ProvinceId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
        [Display(Name = "Agent ID")]
        public int AgentId { get; set; }
        [Display(Name = "Teamleader")]
        public string TeamLeaderName { get; set; }


    }
}
