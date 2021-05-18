using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Core.Leads.Activity
{
    public class ActivityCreation
    {
        public static void CaptureLeadActivity(LeadActivity activity, bool overRide = true)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                JazMax.DataAccess.LeadActivityForLead act = new DataAccess.LeadActivityForLead()
                {
                    CoreUserId = activity.CoreUserId,
                    DateCreated = DateTime.Now,
                    LeadActivityId = activity.LeadActivityId,
                    LeadId = activity.LeadId,
                    IsDeleted = false,
                    IsSystem = activity.IsSystem,
                    Description = activity.Description
                };
                db.LeadActivityForLeads.Add(act);
                db.SaveChanges();

                if (overRide)
                {
                    JazMax.Core.Leads.Status.LeadStatusLogic.ChangeLeadStatus(activity.LeadId);
                }
            }
        }
    }

    public class LeadActivity
    {
        public int LeadActivityId { get; set; }
        public int LeadId { get; set; }
        public int CoreUserId { get; set; }
        public bool IsSystem { get; set; }
        public string Description { get; set; }
    }

    //Lead ActivityType 1 - New Lead Created
    //Lead 
}
