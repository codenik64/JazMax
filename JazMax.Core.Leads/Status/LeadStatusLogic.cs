using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Core.Leads.Status
{
    public class LeadStatusLogic
    {
        //Move Lead Status
        /*
         1 = New
         2 = WIP
         3 = Closed
        */
        public static void ChangeLeadStatus(int LeadId)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                int CountUserActivity = (from b in db.LeadActivityForLeads
                                         where b.IsSystem == false && b.LeadId == LeadId
                                         select b).Count();

                if (CountUserActivity > 0)
                {
                    JazMax.DataAccess.Lead lead = db.Leads.FirstOrDefault(x => x.LeadId == LeadId);
                    lead.LeadStatusId = 2;
                    db.SaveChanges();
                }

            }
        }

        public static void CloseLead(int LeadId, Activity.LeadActivity model)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var Lead = db.Leads?.FirstOrDefault(x => x.LeadId == LeadId);

                if(Lead != null)
                {
                    Lead.IsCompleted = true;
                    Lead.LeadStatusId = 3; //Closed Lead
                }
                db.SaveChanges();

                JazMax.Core.Leads.Activity.ActivityCreation.CaptureLeadActivity(model, false);
                
            }
        }
    }
}
