using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Leads.Reports
{
    public class LeadsByActivity
    {
        public int NumberOfLeads { get; set; }
        public int LeadId { get; set; }
        public int CoreBranchId { get; set; }
        public int LeadStatusId { get; set; }
    }

    public class LeadsByActivityView
    {
        public int NumberOfLeads { get; set; }
        public int LeadId { get; set; }
        public int CoreBranchId { get; set; }
        public int LeadStatusId { get; set; }
        public string BranchName { get; set; }
        public string LeadStatusName { get; set; }
    }
}
