using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Leads.Reports
{
    public class LeadsByAgent
    {
        public int AgentId { get; set; }
        public int NumberOfLeads { get; set; }
       
    }

    public class LeadByAgentView
    {
        public int AgentId { get; set; }
        public int NumberOfLeads { get; set; }
        public string AgentName { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; }
    }

    public class LeadByAgentFilter
    {
        public int AgentId { get; set; }
        public int BranchId { get; set; }
        public int LeadStatusId { get; set; }
        public bool ShowResult { get; set; }
    }
}
