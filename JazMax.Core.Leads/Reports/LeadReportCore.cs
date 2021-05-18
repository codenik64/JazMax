using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.Leads.Reports;

namespace JazMax.Core.Leads.Reports
{
    public class LeadReportCore
    {
        #region Leads By Branch
        public static List<LeadActivityByBranch> LeadByBranch(LeadActivityByBranchFilter filter)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var query = db.VwGetLeadsByActivities.Select(x => new LeadActivityByBranch
                {
                    BranchName = x.BranchName,
                    NumberOfActivities = (int)x.NumberOfActivities,
                    CoreBranchId = x.CoreBranchId,
                    DateCreated = x.DateCreated,
                    LeadID = x.LeadId,
                    LeadStatusId = x.LeadStatusId,
                    StatusName = x.StatusName
                }).ToList().AsQueryable();

                var mine = query;

                if (filter.CoreBranchId > 0)
                {
                    mine = mine.Where(x => x.CoreBranchId == filter.CoreBranchId);
                }

                if (filter.LeadStatusId > 0)
                {
                    mine = mine.Where(x => x.LeadStatusId == filter.LeadStatusId);
                }

                filter.TotalResults = mine.Count();
                return mine.ToList();
            }
        }
        #endregion

        #region Leads By Agent
        public static List<LeadActivityByAgent> LeadByAgentNew(LeadActivityByAgentFilter filter)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var sqlParams = new SqlParameter[] { new SqlParameter { ParameterName = "@CoreUserId", Value = filter.CoreUserId } };
                return db.Database.SqlQuery<LeadActivityByAgent>($"SPLeadActivityByAgent @CoreUserId", sqlParams).ToList();
            }
        }
        #endregion

        #region Leads Closure 
        public static List<LeadClosedReport> LeadClosedReport(LeadClosedReportFilter filter)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var sqlParams = new SqlParameter[] 
                {
                    new SqlParameter { ParameterName = "@LeadStatusId", Value = filter.LeadStatusId },
                    new SqlParameter { ParameterName = "@CoreBranchId", Value = filter.BranchId },
                    new SqlParameter { ParameterName = "@DateFrom", Value = filter.DateFrom },
                    new SqlParameter { ParameterName = "@DateTo", Value = filter.DateTo },
                };
                return db.Database.SqlQuery<LeadClosedReport>($"SPLeadClosedReport @LeadStatusId, @CoreBranchId, @DateFrom, @DateTo", sqlParams).ToList();
            }
        }
        #endregion

        #region Leads Property
        public static List<LeadsByProperty> LeadsByProperty(LeadsByPropertyFilter filter)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var sqlParams = new SqlParameter[]
                {
                    new SqlParameter { ParameterName = "@LeadStatusId", Value = filter.LeadStatusId },
                    new SqlParameter { ParameterName = "@CoreBranchId", Value = filter.BranchId },
                    new SqlParameter { ParameterName = "@DateFrom", Value = filter.DateFrom },
                    new SqlParameter { ParameterName = "@DateTo", Value = filter.DateTo },
                };
                return db.Database.SqlQuery<LeadsByProperty>($"SPLeadByProperty @LeadStatusId, @CoreBranchId, @DateFrom, @DateTo", sqlParams).ToList();
            }
        }
        #endregion
    }
}
