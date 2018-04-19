using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JazMax.Web.ViewModel.UserAccountView;

namespace JazMax.BusinessLogic.UserAccounts
{
    public class AgentService
    {
        private static JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();

        public IQueryable<AgentDetailsView> GetAll()
        {
            var query = from a in db.CoreUsers
                        join b in db.CoreAgents
                        on a.CoreUserId equals b.CoreUserId
                        join c in db.CoreBranches
                        on b.CoreBranchId equals c.BranchId
                        join d in db.VwGetTeamLeadersInformations
                        on c.CoreTeamLeaderId equals d.CoreTeamLeaderId
                        
                        select new AgentDetailsView
                        {
                            AgentId = b.CoreAgentId,
                            BranchName = c.BranchName,
                            EmailAddress = a.EmailAddress,
                            IsActive = a.IsActive,
                            BranchId = (int)b.CoreBranchId,
                            CellPhone = a.CellPhone,
                            CoreTeamLeaderId = c.CoreTeamLeaderId,
                            CoreUserId = a.CoreUserId,
                            FirstName = a.FirstName,
                            IDNumber = a.IdNumber,
                            LastName = a.LastName,
                            MiddleName = a.MiddleName,
                            PhoneNumber = a.PhoneNumber,
                            ProvinceId = c.ProvinceId,
                            TeamLeaderName = d.FirstName + " " + d.LastName
                        };

            return query.AsQueryable();
        }

        public IQueryable<AgentDetailsView> GetMyAgents(int teamLeaderId)
        {
            return GetAll().Where(x => x.CoreTeamLeaderId == (int)teamLeaderId).AsQueryable();

        }

        public IQueryable<AgentDetailsView> GetMyAgentInProvince(int proId)
        {
            return GetAll().Where(x => x.ProvinceId == (int)proId).AsQueryable();

        }

        public List<AgentDetailsView> GetMyAgentInBranch(int branchId)
        {
            return GetAll().Where(x => x.BranchId == (int)branchId).ToList();

        }


    }
}
