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
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhone { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> GenderId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int CoreUserTypeId { get; set; }
        public CapturePAView CapturePAView { get; set; }
        public CaptureTeamLeader CaptureTeamLeader { get; set; }
        public CaptureAgent CaptureAgent { get; set; }
        public Nullable<int> BranchIdCapture { get; set; }
    }
}
