using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.Leads;
namespace JazMax.Core.Leads.Activity
{
    public class ActivityLogic
    {
        private static int LeadId = 0;

        public ActivityLogic(int LeadID = 0)
        {
            LeadId = LeadID;
        }

        public static List<LeadActivites> GetLeadActivities()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var query = (from t in db.LeadActivityForLeads
                             join b in db.LeadActivities
                             on t.LeadActivityId equals b.LeadActivityId
                             join c in db.CoreUsers
                             on t.CoreUserId equals c.CoreUserId
                             where t.LeadId == LeadId
                             && t.CoreUserId != -999
                             && t.IsDeleted == false
                             select new LeadActivites
                             {
                                 ActivityTypeName = b.ActivityName,
                                 AgentName = c.FirstName + " " + c.LastName,
                                 CoreUserID = t.CoreUserId,
                                 DateCreated = t.DateCreated,
                                 Description = t.Description,
                                 IsDeleted = t.IsDeleted,
                                 IsSystem = t.IsSystem,
                                 LeadActivityForLeadID = t.LeadActivityForLeadId,
                                 LeadId = t.LeadId
                             }).
                             Union
                             (from q in db.LeadActivityForLeads
                              join w in db.LeadActivities
                              on q.LeadActivityId equals w.LeadActivityId
                              where q.CoreUserId == -999
                              && q.LeadId == LeadId
                              && q.IsDeleted == false
                              select new LeadActivites
                              {
                                  ActivityTypeName = w.ActivityName,
                                  AgentName = "System Activity",
                                  CoreUserID = q.CoreUserId,
                                  DateCreated = q.DateCreated,
                                  Description = q.Description,
                                  IsDeleted = q.IsDeleted,
                                  IsSystem = q.IsSystem,
                                  LeadActivityForLeadID = q.LeadActivityForLeadId,
                                  LeadId = q.LeadId

                              })?.OrderByDescending(x =>x.DateCreated)?.ToList();

                return query;
            }
        }

        public static LeadActivites GetLastLeadActivity()
        {
            return GetLeadActivities()?.FirstOrDefault();
        }
    }

    
}
