using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.Leads;

namespace JazMax.Core.Leads.Reminder
{
    public class ReminderLogic
    {
        public IQueryable<LeadRemindersList> GetMyLeadReminders(LeadReminderSearch index)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var query = (from t in db.LeadReminders
                             join b in db.CoreUsers
                             on t.CoreUserId equals b.CoreUserId
                             join c in db.Leads
                             on t.LeadId equals c.LeadId
                             join d in db.CoreBranches
                             on c.CoreBranchId equals d.BranchId
                             where t.WebJobCompleted == false
                             select new LeadRemindersList
                             {
                                 AgentName = b.FirstName + " " + b.LastName,
                                 BranchId = c.CoreBranchId,
                                 CoreUserId = t.CoreUserId,
                                 DateCreated = t.DateCreated,
                                 Description = t.Description,
                                 LeadId = t.LeadId,
                                 ProvinceId = (int)d.ProvinceId,
                                 ReminderDate = t.ReminderDate
                             }).ToList().AsQueryable();

                var mine = query.AsQueryable();

                if (index.CoreUserId > 0)
                {
                    mine = mine.Where(x => x.CoreUserId == index.CoreUserId);
                }

                if (index.BranchId > 0)
                {
                    mine = mine.Where(x => x.BranchId == index.BranchId);
                }

                if (index.ProvinceId > 0)
                {
                    mine = mine.Where(x => x.ProvinceId == index.ProvinceId);
                }

                return mine;
            }

        }
        public class LeadReminderSearch
        {
            public int CoreUserId { get; set; }
            public int BranchId { get; set; }
            public int ProvinceId { get; set; }
        }

    }
}
