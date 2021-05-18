using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Leads.Reports
{
    public class LeadActivityByBranch
    {
        public int LeadID { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
        [Display(Name = "Number Of Activities")]
        public int NumberOfActivities { get; set; }
        public int CoreBranchId { get; set; }
        public int LeadStatusId { get; set; }
        [Display(Name = "Lead Status")]
        public string StatusName { get; set; }
    }

    public class LeadActivityByBranchFilter
    {
        [Display(Name = "Branch")]
        public int CoreBranchId { get; set; }
        [Display(Name = "Lead Status")]
        public int LeadStatusId { get; set; }
        public bool ShowReport { get; set; }
        public List<LeadActivityByBranch> Results { get; set; }
        public int TotalResults { get; set; }
    }

    public class LeadActivityByAgent
    {
        public int LeadID { get; set; }
        public DateTime DateCreated { get; set; }
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
        [Display(Name = "Lead Status")]
        public string StatusName { get; set; }
        [Display(Name = "Number Of Activities")]
        public int NumberOfActivities { get; set; }
        public int CoreBranchId { get; set; }
        public int LeadStatusId { get; set; }
        public int CoreUserId { get; set; }
    }

    public class LeadActivityByAgentFilter
    {
        [Display(Name = "Agent")]
        public int CoreUserId { get; set; }
        public int LeadStatusId { get; set; }
        public int BranchId { get; set; }
        public bool ShowReport { get; set; }
        public List<LeadActivityByAgent> Result { get; set; }
    }

    public class LeadClosedReport
    {
        public int LeadID { get; set; }
        public DateTime DateCreated { get; set; }
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
        [Display(Name = "Lead Status")]
        public string StatusName { get; set; }
        [Display(Name = "Number Of Activities")]
        public int NumberOfActivities { get; set; }
        public int CoreBranchId { get; set; }
        public int LeadStatusId { get; set; }
        public int CoreUserId { get; set; }
        [Display(Name = "Agent")]
        public string AgentName { get; set; }
    }

    public class LeadClosedReportFilter
    {
        [Display(Name = "Lead Status")]
        public int LeadStatusId { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }
        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }
        public bool ShowReport { get; set; }
        public List<LeadClosedReport> Result { get; set; }
    }

    public class LeadsByProperty
    {
        public int TotalLeads { get; set; }
        public string PropertyFriendlyName { get; set; }
        public int PropertyListingId { get; set; }
        public string PropertyType { get; set; }
    }
    public class LeadsByPropertyFilter
    {
        [Display(Name = "Lead Status")]
        public int LeadStatusId { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }
        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }
        public bool ShowReport { get; set; }
        public List<LeadsByProperty> Result { get; set; }
    }


}
