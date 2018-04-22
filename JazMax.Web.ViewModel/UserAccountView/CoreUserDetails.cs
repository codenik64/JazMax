using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class CoreUserDetails
    {

        public int CoreUserId { get; set; }
        public int CoreUserTypeId { get; set; }
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
        [Display(Name = "Gender")]
        public string GenderId { get; set; }
        [Display(Name = "Type")]
        public string UserType { get; set; }
        [Display(Name = "Status")]
        public bool isActive { get; set; }

        public UserBranchDetails UserBranchDetails { get; set; }
        public UserProvinceDetails UserProvinceDetails { get; set; }
        public UserTeamLeaderDetails UserTeamLeaderDetails { get; set; }
        public List<ChangeLog.EditLogView> UserEditLog { get; set; }
    }
}
