using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class CoreUserView
    {
        [Key]
        public int CoreUserId { get; set; }
        [Display(Name ="First Name")]
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
        public Nullable<int> GenderId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        [Display(Name = "User Type")]
        public int CoreUserTypeId { get; set; }
        public CapturePAView CapturePAView { get; set; }
        public CaptureTeamLeader CaptureTeamLeader { get; set; }
        public CaptureAgent CaptureAgent { get; set; }
        [Display(Name = "Branch")]
        public Nullable<int> BranchIdCapture { get; set; }
    }
}
