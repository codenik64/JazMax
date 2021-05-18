using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Leads
{
    public class LeadRemindersList
    {
        public int LeadId { get; set; }
        public int CoreUserId { get; set; }
        public string AgentName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ReminderDate { get; set; }
        public string Description { get; set; }
        public int BranchId { get; set; }
        public int ProvinceId { get; set; }
    }
}
