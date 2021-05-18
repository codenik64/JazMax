using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Core.SystemHelpers.Model
{
    //JazMax User Identity Helper V2.0 - Simple
    public class User
    {
        private string Username { get; set; }
        
        public User(string Username)
        {
            this.Username = Username;
            GetUser();

        }

        public UserData GetUser()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var query = (from t in db.CoreUsers
                             join b in db.CoreUserInTypes
                             on t.CoreUserId equals b.CoreUserId
                             where b.CoreUserTypeId == (int)JazMax.Common.Enum.UserType.Agent
                             join c in db.CoreAgents
                             on t.CoreUserId equals c.CoreUserId
                             join e in db.CoreBranches
                             on c.CoreBranchId equals e.BranchId
                             select new UserData
                             {
                                 CoreUserId = t.CoreUserId,
                                 AgentId = c.CoreAgentId,
                                 BranchId = e.BranchId,
                                 ProvinceId = (int)e.ProvinceId,
                                 TeamLeaderId = (int)e.CoreTeamLeaderId
                             }).FirstOrDefault();
                             //Union(from t in db.CoreUsers
                             //         join b in db.CoreUserInTypes
                             //         on t.CoreUserId equals b.CoreUserId
                             //         where b.CoreUserTypeId == (int)JazMax.Common.Enum.UserType.TeamLeader
                             //         join c in db.CoreTeamLeaders
                             //         on t.CoreUserId equals c.CoreUserId
                             //         join e in db.CoreBranches
                             //         on c.CoreTeamLeaderId equals e.CoreTeamLeaderId
                             //         select new UserData
                             //         {
                             //             CoreUserId = t.CoreUserId,
                             //             BranchId = e.BranchId,
                             //             ProvinceId = (int)e.ProvinceId,
                             //             TeamLeaderId = (int)e.CoreTeamLeaderId
                             //         })?.FirstOrDefault();

                return query;

            }
        }
        public class UserData
        {
            public int? CoreUserId { get; set; }
            public int? AgentId { get; set; }
            public int? TeamLeaderId { get; set; }
            public int? ProvinceId { get; set; }
            public int? BranchId { get; set; }
        }

    }
}
