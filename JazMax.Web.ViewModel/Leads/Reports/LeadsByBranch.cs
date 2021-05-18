using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Leads.Reports
{
    public class LeadByBranch
    {
        public int CoreBranchId { get; set; }
        public string BranchName { get; set; }
        public int NumberOfLeads { get; set; }
  
    }

    public class LeadsByBranchSearch
    {
        public int BranchId { get; set; }
        public int LeadStatusId { get; set; }
        public bool ShowResult { get; set; }
    }
}
