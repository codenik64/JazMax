using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class CoreBranchView
    {
        public int BranchId { get; set; }
        public Nullable<int> CoreTeamLeaderId { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string BranchName { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Suburb { get; set; }
    }
}
