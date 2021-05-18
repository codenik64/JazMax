using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JazMax.Core.Leads.LeadPerfomance
{
    public class LeadProcessor
    {

        #region Get RawLead Count (Unprocessed)
        public int RawLeadCount()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var RawCount = (from rawleads in db.LeadRawDatas
                                where rawleads.ServiceCompleted == false
                                select rawleads).ToList();

                return RawCount.Count();
            }
         
        }
        #endregion

        #region Get Processed Leads (Potential Leads)
        public int ProcessedLeadCount()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var leadCount = (from leads in db.Leads
                                 where leads.ServiceCompleted == true
                                 select leads).ToList();

                return leadCount.Count();
            }
        }
        #endregion

        #region Manually Captured LeadCount

        #region Get manually Captured leads for user
        public int ManualCaptureCountUserId()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {


                var Finduser = db.CoreUsers.FirstOrDefault(x => x.EmailAddress == HttpContext.Current.User.Identity.Name);

                var ManualCount = (from leads in db.LeadRawDatas
                                   where leads.IsManualCapture == true
                                   && leads.CoreUserId == Finduser.CoreUserId
                                   select leads).ToList();

                return ManualCount.Count();
            }
        }
        #endregion

        public int ManualCaptureCount()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var ManualCount = (from leads in db.LeadRawDatas
                                 where leads.IsManualCapture == true
                                 select leads).ToList();

                return ManualCount.Count();
            }
        }
        #endregion

        #region System Processed LeadCount
        public int SystemProcessedCount()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var SystemCount = (from leads in db.LeadRawDatas
                                   where leads.IsManualCapture == false
                                   && leads.CoreUserId == -999
                                   select leads).ToList();

                return SystemCount.Count();
            }
        }
        #endregion

        #region Potential Lead Prospects Count
        public int LeadProspectsCount()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var LeadProspectsCount = (from leads in db.LeadProspects
                                            select leads).ToList();

                return LeadProspectsCount.Count();
            }
        }
        #endregion

        #region NewLeadsCount
        public int NewLeadsCount()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var NewLeadsCount = (from leads in db.Leads
                                     where leads.LeadStatusId == 1
                                     select leads).ToList();

                return NewLeadsCount.Count();
            }
        }
        #endregion

        #region Lead WorkInProgress(WIP)
        public int WorkInProgressCount()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var WorkInProgressCount = (from leads in db.Leads
                                           where leads.LeadStatusId == 2
                                          select leads).ToList();

                return WorkInProgressCount.Count();
            }
        }
        #endregion

        #region LeadsClosed
        public int LeadsClosedCount()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var LeadsClosedCount = (from leads in db.Leads
                                        where leads.LeadStatusId == 3
                                          select leads).ToList();

                return LeadsClosedCount.Count();
            }
        }
        #endregion

        #region Leads for The Last 7 days
        public int Last7Days()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var date = DateTime.Now;
                var Last7Days = (from leads in db.Leads
                                 select leads).ToList();
                var count = Last7Days.Where(x => x.DateCreated >= date.AddDays(-7));

                return count.Count();
            }
        }
        #endregion

        #region Leads for The Last Month
        public int LastMonth()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var date = DateTime.Now;
                var LastMonth = (from leads in db.Leads
                                 select leads).ToList();
                var count = LastMonth.Where(x => x.DateCreated >= date.AddMonths(-1));

                return count.Count();
            }
        }
        #endregion

        


    }
}
