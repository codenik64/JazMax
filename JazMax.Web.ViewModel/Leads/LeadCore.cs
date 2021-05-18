using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Leads
{
    public class LeadCore
    {
        public int LeadId { get; set; }
        public int LeadTypeId { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public int PropertyListingId { get; set; }
        public bool HasLinkedLeads { get; set; }
        public List<LeadAgents> LeadAgents { get; set; }
        public LeadProspect LeadProspect { get; set; }
        [Display(Name = "Type")]
        public string LeadTypeName { get; set; }
        [Display(Name = "Source")]
        public string LeadSourceName { get; set; }
        [Display(Name = "Status")]
        public string LeadStatusName { get; set; }
        public List<LeadActivites> LeadActivities { get; set; }
        public List<LinkedLeadsList> LinkedLeadsList { get; set; }
        public List<LeadReminder> LeadReminder { get; set; }
        public LeadProperty LeadProperty { get; set; }
        public bool IsClosed { get; set; }
    }

    public class LeadAgents
    {
        public int LeadId { get; set; }
        public int AgentId { get; set; }
        public int CoreUserId { get; set; }
        public string FriendlyName { get; set; }
    }

    public class LeadProspect
    {
        public int LeadId { get; set; }
        [Display(Name = "Name")]
        public string FullName { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Comments { get; set; }
    }

    public class LeadActivites
    {
        public int LeadId { get; set; }
        public int LeadActivityForLeadID { get; set; }
        [Display(Name = "Activity")]
        public string ActivityTypeName { get; set; }
        public string Description { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public int CoreUserID { get; set; }
        [Display(Name = "User")]
        public string AgentName { get; set; }
        public bool IsSystem { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class LinkedLeadsList
    {
        public int LeadId { get; set; }
        public int LinkedLeadId { get; set; }
        public DateTime DateOfLinked { get; set; }
    }

    public class LeadReminder
    {
        public int LeadId { get; set; }
        public int CoreUserId { get; set; }
        [Display(Name = "User")]
        public string AgentName { get; set; }
        public string Description { get; set; }
        [Display(Name = "Reminder Date")]
        public DateTime DateReminder { get; set; }
        [Display(Name = "User")]
        public DateTime DateCreated { get; set; }
    }

    public class LeadProperty
    {
        public int LeadId { get; set; }
        public string FriednlyName { get; set; }
        public string PropertyType { get; set; }
        public decimal Price { get; set; }
        public string PropertyPriceType { get; set; }
        public string Province { get; set; }
    }
}
