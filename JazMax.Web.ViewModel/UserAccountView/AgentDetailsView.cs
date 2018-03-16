using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class AgentDetailsView
    {
        public int CoreUserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string IDNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhone { get; set; }
        public string EmailAddress { get; set; }
        public int BranchId { get; set; }
        public Nullable<int> CoreTeamLeaderId { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string BranchName { get; set; }
        public int AgentId { get; set; }
        public string TeamLeaderName { get; set; }


    }
}
