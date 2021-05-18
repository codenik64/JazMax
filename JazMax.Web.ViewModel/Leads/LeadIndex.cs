using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Leads
{
    public class LeadIndex
    {
        [Display(Name="LeadID")]
        public int LeadId { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Type")]
        public string LeadType { get; set; }
        [Display(Name = "Source")]
        public string LeadSource { get; set; }
        [Display(Name = "Status")]
        public string LeadStatus { get; set; }
        [Display(Name = "Property Item")]
        public string PropertyListingFriendlyName { get; set; }
        [Display(Name = "Prospect")]
        public string ProspectName { get; set; }
        [Display(Name = "Last Activity")]
        public string LastActivity { get; set; }
        public List<LeadAgents> LeadAgents { get; set; }
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public int LeadStatusId { get; set; }
        public int LeadTypeId { get; set; }
    }

    public class LeadIndexSearch
    {
        [Display(Name = "LeadID")]
        public int LeadId { get; set; }
        [Display(Name = "Lead Type")]
        public int LeadTypeId { get; set; }
        [Display(Name = "Lead Status")]
        public int LeadStatusId { get; set; }
        [Display(Name = "Prospect")]
        public string ProspectName { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        [Display(Name = "Agent")]
        public int AngentId { get; set; }
        [Display(Name = "Order By Date")]
        public int OrderBy { get; set; }
        public bool ShowResult { get; set; }
        public IQueryable<LeadIndex> LeadIndex { get; set; }
        public List<int> BranchIdList { get; set; }
    }
}
