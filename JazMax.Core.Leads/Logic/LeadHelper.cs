using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.Leads;
namespace JazMax.Core.Leads.Logic
{
    public class LeadHelper
    {
        public int LeadId = 0;

        public LeadHelper(int LeadId = 0)
        {
            this.LeadId = LeadId;
        }

        #region Get A Single Lead
        public LeadCore GetLead(int OverRiderId = 0)
        {
            if (OverRiderId > 0)
            {
                LeadId = OverRiderId;
            }
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {

                var okay = db.Leads.FirstOrDefault(x => x.LeadId == LeadId).LeadSourceId;
                LeadCore query = null;

                #region Has Source
                if (okay != -999)
                {

                    query = (from a in db.Leads
                             join b in db.LeadTypes
                             on a.LeadTypeId equals b.LeadTypeId
                             join c in db.LeadSources
                             on a.LeadSourceId equals c.LeadSourceId
                             join d in db.LeadStatus
                             on a.LeadStatusId equals d.LeadStatusId
                             where a.LeadId == LeadId && a.LeadSourceId != -999
                             select new LeadCore
                             {
                                 DateCreated = a.DateCreated,
                                 HasLinkedLeads = a.HasLinkedLead,
                                 LeadId = a.LeadId,
                                 LeadSourceName = c.SourceName,
                                 LeadStatusName = d.StatusName,
                                 LeadTypeName = b.TypeName,
                                 PropertyListingId = a.PropertyListingId,
                                 LeadTypeId = a.LeadTypeId,
                                 IsClosed = a.IsCompleted,
                                 LeadReminder = (from t in db.LeadReminders
                                                 join b in db.CoreUsers
                                                 on t.CoreUserId equals b.CoreUserId
                                                 where t.LeadId == LeadId
                                                 select new LeadReminder
                                                 {
                                                     CoreUserId = t.CoreUserId,
                                                     DateReminder = t.ReminderDate,
                                                     Description = t.Description,
                                                     LeadId = t.LeadId,
                                                     DateCreated = t.DateCreated,
                                                     AgentName = b.FirstName + " " + b.LastName
                                                 }).ToList(),
                                 LeadAgents = (from a in db.LeadAgents
                                               join b in db.CoreAgents
                                               on a.AgentId equals b.CoreAgentId
                                               join c in db.CoreUsers
                                               on b.CoreUserId equals c.CoreUserId
                                               where a.LeadId == LeadId
                                               select new LeadAgents
                                               {
                                                   AgentId = a.AgentId,
                                                   CoreUserId = (int)b.CoreUserId,
                                                   FriendlyName = c.FirstName + " " + c.LastName,
                                                   LeadId = a.LeadId
                                               }).ToList(),
                                 LeadProspect = (from a in db.LeadProspects
                                                 where a.LeadId == LeadId
                                                 select new LeadProspect
                                                 {
                                                     Comments = a.Comments,
                                                     ContactNumber = a.ContactNumber,
                                                     Email = a.Email,
                                                     FullName = a.FullName,
                                                     LeadId = a.LeadId
                                                 }).FirstOrDefault(),
                                 LeadProperty = (from prop in db.PropertyListings
                                                 join proptype in db.PropertyTypes
                                                 on prop.PropertyTypeId equals proptype.PropertyTypeId
                                                 join prov in db.CoreProvinces
                                                 on prop.ProvinceId equals prov.ProvinceId
                                                 join proprice in db.PropertyListingPricingTypes
                                                 on prop.PropertyListingPricingTypeId equals proprice.PropertyListingPricingTypeId
                                                 select new LeadProperty
                                                 {
                                                     FriednlyName = prop.FriendlyName,
                                                     LeadId = a.LeadId,
                                                     Price = prop.Price,
                                                     PropertyPriceType = proprice.TypeName,
                                                     PropertyType =proptype.TypeName,
                                                     Province = prov.ProvinceName
                                                 }).FirstOrDefault(),
                                 LeadActivities = (from t in db.LeadActivityForLeads
                                                   join b in db.LeadActivities
                                                   on t.LeadActivityId equals b.LeadActivityId
                                                   join c in db.CoreUsers
                                                   on t.CoreUserId equals c.CoreUserId
                                                   where t.LeadId == LeadId
                                                   && t.CoreUserId != -999
                                                   && t.IsDeleted == false
                                                   orderby t.DateCreated descending
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
                                                   }).Union(from q in db.LeadActivityForLeads
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

                                                            }).ToList()
                             })?.FirstOrDefault();
                }
                #endregion

                #region Has No Source
                else
                {
                    query = (from a in db.Leads
                             join b in db.LeadTypes
                             on a.LeadTypeId equals b.LeadTypeId
                             join d in db.LeadStatus
                             on a.LeadStatusId equals d.LeadStatusId
                             where a.LeadId == LeadId && a.LeadSourceId == -999
                             select new LeadCore
                             {
                                 DateCreated = a.DateCreated,
                                 HasLinkedLeads = a.HasLinkedLead,
                                 LeadId = a.LeadId,
                                 LeadSourceName = "Unknown",
                                 LeadStatusName = d.StatusName,
                                 LeadTypeName = b.TypeName,
                                 PropertyListingId = a.PropertyListingId,
                                 LeadTypeId = a.LeadTypeId,
                                 IsClosed = a.IsCompleted,
                                 LeadReminder = (from t in db.LeadReminders
                                                 join b in db.CoreUsers
                                                 on t.CoreUserId equals b.CoreUserId
                                                 where t.LeadId == LeadId
                                                 select new LeadReminder
                                                 {
                                                     CoreUserId = t.CoreUserId,
                                                     DateReminder = t.ReminderDate,
                                                     Description = t.Description,
                                                     LeadId = t.LeadId,
                                                     DateCreated = t.DateCreated,
                                                     AgentName = b.FirstName + " " + b.LastName
                                                 }).ToList(),
                                 LeadAgents = (from a in db.LeadAgents
                                               join b in db.CoreAgents
                                               on a.AgentId equals b.CoreAgentId
                                               join c in db.CoreUsers
                                               on b.CoreUserId equals c.CoreUserId
                                               where a.LeadId == LeadId
                                               select new LeadAgents
                                               {
                                                   AgentId = a.AgentId,
                                                   CoreUserId = (int)b.CoreUserId,
                                                   FriendlyName = c.FirstName + " " + c.LastName,
                                                   LeadId = a.LeadId
                                               }).ToList(),
                                 LeadProperty = (from prop in db.PropertyListings
                                                 join proptype in db.PropertyTypes
                                                 on prop.PropertyTypeId equals proptype.PropertyTypeId
                                                 join prov in db.CoreProvinces
                                                 on prop.ProvinceId equals prov.ProvinceId
                                                 join proprice in db.PropertyListingPricingTypes
                                                 on prop.PropertyListingPricingTypeId equals proprice.PropertyListingPricingTypeId
                                                 select new LeadProperty
                                                 {
                                                     FriednlyName = prop.FriendlyName,
                                                     LeadId = a.LeadId,
                                                     Price = prop.Price,
                                                     PropertyPriceType = proprice.TypeName,
                                                     PropertyType = proptype.TypeName,
                                                     Province = prov.ProvinceName
                                                 }).FirstOrDefault(),
                                 LeadProspect = (from a in db.LeadProspects
                                                 where a.LeadId == LeadId
                                                 select new LeadProspect
                                                 {
                                                     Comments = a.Comments,
                                                     ContactNumber = a.ContactNumber,
                                                     Email = a.Email,
                                                     FullName = a.FullName,
                                                     LeadId = a.LeadId
                                                 }).FirstOrDefault(),
                                 LeadActivities = (from t in db.LeadActivityForLeads
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
                                                   }).OrderByDescending(x =>x.DateCreated).Union(from q in db.LeadActivityForLeads
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

                                                            }).ToList()
                                 //   LinkedLeadsList = null
                             })?.FirstOrDefault();
                }
                #endregion

                if (query.HasLinkedLeads)
                {
                    query.LinkedLeadsList = GetLinkedLeads();
                }

                return query;
            }

        }

        private List<LeadAgents> GetAgentsForLead()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var query = (from a in db.LeadAgents
                             join b in db.CoreAgents
                             on a.AgentId equals b.CoreAgentId
                             join c in db.CoreUsers
                             on b.CoreUserId equals c.CoreUserId
                             where a.LeadId == LeadId
                             select new LeadAgents
                             {
                                 AgentId = a.AgentId,
                                 CoreUserId = (int)b.CoreUserId,
                                 FriendlyName = c.FirstName + " " + c.LastName,
                                 LeadId = a.LeadId
                             })?.ToList();

                return query;
            }
        }

        private LeadProspect GetLeadProspectForLead()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var query = (from a in db.LeadProspects
                             where a.LeadId == LeadId
                             select new LeadProspect
                             {
                                 Comments = a.Comments,
                                 ContactNumber = a.ContactNumber,
                                 Email = a.Email,
                                 FullName = a.FullName,
                                 LeadId = a.LeadId
                             })?.FirstOrDefault();

                return query;
            }
        }

        private List<LinkedLeadsList> GetLinkedLeads()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                int? aaa = (from t in db.LeadWithSameProspects
                            join b in db.LeadWithSameProspectLinks
                            on t.LeadWithSameProspectId equals b.LeadWithSameProspectId
                            where b.LinkedLeadId == LeadId
                            select t.LeadId)?.FirstOrDefault();

                if (aaa == null)
                {
                    aaa = 0;
                }

                var query = (from t in db.LeadWithSameProspects
                             join b in db.LeadWithSameProspectLinks
                             on t.LeadWithSameProspectId equals b.LeadWithSameProspectId
                             where t.LeadId == LeadId
                             select new LinkedLeadsList
                             {
                                 LeadId = t.LeadId,
                                 LinkedLeadId = b.LinkedLeadId,
                                 DateOfLinked = DateTime.Now  // fix later
                             }).Union(from t in db.LeadWithSameProspects
                                      join b in db.LeadWithSameProspectLinks
                                      on t.LeadWithSameProspectId equals b.LeadWithSameProspectId
                                      where t.LeadId == (int)aaa
                                      select new LinkedLeadsList
                                      {
                                          LeadId = t.LeadId,
                                          LinkedLeadId = b.LinkedLeadId,
                                          DateOfLinked = DateTime.Now  // fix later
                                      })?.ToList();
                return query;
            }
        }
        #endregion

        #region Get List Of Leads
        public IQueryable<LeadIndex> GetLeadIndex(int BranchId = 0, int AgentId = 0)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                IQueryable<LeadIndex> query = (from a in db.Leads
                                               join b in db.LeadTypes
                                               on a.LeadTypeId equals b.LeadTypeId
                                               join c in db.LeadSources
                                               on a.LeadSourceId equals c.LeadSourceId
                                               join d in db.LeadStatus
                                               on a.LeadStatusId equals d.LeadStatusId
                                               join e in db.PropertyListings
                                               on a.PropertyListingId equals e.PropertyListingId
                                               join f in db.CoreBranches
                                               on a.CoreBranchId equals f.BranchId
                                               join g in db.LeadProspects
                                               on a.LeadId equals g.LeadId
                                               select new LeadIndex
                                               {
                                                   DateCreated = a.DateCreated,
                                                   LeadId = a.LeadId,
                                                   LeadSource = c.SourceName,
                                                   LeadStatus = d.StatusName,
                                                   LeadType = b.TypeName,
                                                   PropertyListingFriendlyName = e.FriendlyName,
                                                   BranchName = f.BranchName,
                                                   ProspectName = g.FullName,
                                                   BranchId = f.BranchId
                                               }).ToList().AsQueryable();

                foreach (var ash in query)
                {
                    JazMax.Core.Leads.Activity.ActivityLogic o = new Activity.ActivityLogic(ash.LeadId);
                    ash.LastActivity = JazMax.Core.Leads.Activity.ActivityLogic.GetLastLeadActivity()?.ActivityTypeName;
                }

                foreach (var bob in query)
                {
                    LeadId = bob.LeadId;
                    bob.LeadAgents = GetAgentsForLead();
                }
                var mine = query.AsQueryable();

                if (BranchId > 0)
                {
                    mine = mine.Where(x => x.BranchId == BranchId);
                }

                if (AgentId > 0)
                {
                    mine = mine.Where(x => x.LeadAgents.Where(y => y.AgentId == AgentId).Any());
                }

                //List<LeadIndex> lol = new List<LeadIndex>();
                //lol = mine.ToList();
                //return lol;
                return mine;
            }
        }
        #endregion

        #region Get List Of Leads
        public LeadIndexSearch GetLeadIndexNew(LeadIndexSearch index)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                IQueryable<LeadIndex> query = (from a in db.Leads
                                               join b in db.LeadTypes
                                               on a.LeadTypeId equals b.LeadTypeId
                                               join c in db.LeadSources
                                               on a.LeadSourceId equals c.LeadSourceId
                                               join d in db.LeadStatus
                                               on a.LeadStatusId equals d.LeadStatusId
                                               join e in db.PropertyListings
                                               on a.PropertyListingId equals e.PropertyListingId
                                               join f in db.CoreBranches
                                               on a.CoreBranchId equals f.BranchId
                                               join g in db.LeadProspects
                                               on a.LeadId equals g.LeadId
                                               where a.LeadSourceId != -999
                                               select new LeadIndex
                                               {
                                                   DateCreated = a.DateCreated,
                                                   LeadId = a.LeadId,
                                                   LeadSource = c.SourceName,
                                                   LeadStatus = d.StatusName,
                                                   LeadType = b.TypeName,
                                                   PropertyListingFriendlyName = e.FriendlyName,
                                                   BranchName = f.BranchName,
                                                   ProspectName = g.FullName,
                                                   BranchId = f.BranchId,
                                                   LeadStatusId = a.LeadStatusId,
                                                   LeadTypeId = a.LeadTypeId
                                               }).Union(from a in db.Leads
                                                        join b in db.LeadTypes
                                                        on a.LeadTypeId equals b.LeadTypeId
                                                        join d in db.LeadStatus
                                                        on a.LeadStatusId equals d.LeadStatusId
                                                        join e in db.PropertyListings
                                                        on a.PropertyListingId equals e.PropertyListingId
                                                        join f in db.CoreBranches
                                                        on a.CoreBranchId equals f.BranchId
                                                        join g in db.LeadProspects
                                                        on a.LeadId equals g.LeadId
                                                        where a.LeadSourceId == -999
                                                        select new LeadIndex
                                                        {
                                                            DateCreated = a.DateCreated,
                                                            LeadId = a.LeadId,
                                                            LeadSource = "Unknown",
                                                            LeadStatus = d.StatusName,
                                                            LeadType = b.TypeName,
                                                            PropertyListingFriendlyName = e.FriendlyName,
                                                            BranchName = f.BranchName,
                                                            ProspectName = g.FullName,
                                                            BranchId = f.BranchId,
                                                            LeadStatusId = a.LeadStatusId,
                                                            LeadTypeId = a.LeadTypeId
                                                        }).ToList().AsQueryable();

                foreach (var ash in query)
                {
                    JazMax.Core.Leads.Activity.ActivityLogic o = new Activity.ActivityLogic(ash.LeadId);
                    ash.LastActivity = JazMax.Core.Leads.Activity.ActivityLogic.GetLastLeadActivity()?.ActivityTypeName;
                }

                foreach (var bob in query)
                {
                    LeadId = bob.LeadId;
                    bob.LeadAgents = GetAgentsForLead();
                }
                var mine = query.AsQueryable();

                if (index.BranchId > 0)
                {
                    mine = mine.Where(x => x.BranchId == index.BranchId);
                }

                if (index.LeadStatusId > 0)
                {
                    mine = mine.Where(x => x.LeadStatusId == index.LeadStatusId);
                }

                if (index.LeadTypeId > 0)
                {
                    mine = mine.Where(x => x.LeadTypeId == index.LeadTypeId);
                }

                if (index.AngentId > 0)
                {
                    mine = mine.Where(x => x.LeadAgents.Where(y => y.AgentId == index.AngentId).Any());
                }

                if (!string.IsNullOrEmpty(index.ProspectName))
                {
                    mine = mine.Where(x => x.ProspectName == index.ProspectName);
                }

                if (index.BranchIdList != null)
                {
                    mine = mine.Where(x => index.BranchIdList.Contains(x.BranchId));
                }
                index.LeadIndex = mine.AsQueryable();
                index.ShowResult = true;
                return index;
            }
        }
        #endregion
    }
}
