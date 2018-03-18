﻿using System.Collections.Generic;
using System.Linq;
using JazMax.Core.SystemHelpers.Model;


namespace JazMax.Core.SystemHelpers
{
    public class JazMaxIdentityHelper
    {
        private static AzureDataAccess.JazMaxDBProdContext db = new AzureDataAccess.JazMaxDBProdContext();
        public static string UserName { get; set; }

        #region Personal Assisstant
        public static UserInformation GetPAUserInformation(string userName)
        {
            var user = from a in db.CoreUsers
                       join b in db.CorePAs
                       on a.CoreUserId equals b.CoreUserId
                       join c in db.CoreProvinces
                       on b.ProvinceId equals c.ProvinceId
                       where a.EmailAddress == userName
                       select new UserInformation
                       {
                           DisplayName = a.FirstName + " " + a.LastName,
                           Province = c.ProvinceName,
                           ProvinceId = (int)b.ProvinceId
                       };

            return user.FirstOrDefault();
        }
        #endregion

        #region Team Leader
        public static UserInformation GetTeamLeadersInfo(string userName)
        {
            var q =  db.vw_GetTeamLeadersInformation.Where(x => x.EmailAddress == userName).Select(x => new UserInformation
            {
                BranchName = x.BranchName != null ? x.BranchName : "None",
                DisplayName = x.FirstName + " " + x.LastName != null ? x.FirstName + " " + x.LastName : "None",
                Id = x.CoreUserId.ToString(),
                Province = x.ProvinceName != null ? x.ProvinceName : "None"

            }).FirstOrDefault();

            if(q == null)
            {
                q = new UserInformation
                {
                    BranchName = "None",
                    DisplayName = "None",
                    Province = "None"
                };
            }
            return q;
        }

        public List<UserInformation> GetTeamLeaderForProvince(int pId)
        {
            List<int?> teamLeaderId = db.CoreBranches.Where(x =>x.ProvinceId == pId).Select(x => x.CoreTeamLeaderId).ToList();

            var q = from a in db.CoreUsers
                    join b in db.CoreTeamLeaders
                    on a.CoreUserId equals b.CoreUserId
                    where b.CoreProvinceId == pId &&
                    !teamLeaderId.Contains(b.CoreTeamLeaderId)
                    select new UserInformation
                    {
                        DisplayName = a.FirstName + " " + a.LastName,
                        Id = b.CoreTeamLeaderId.ToString()
                    };

            return q.ToList();
        }
       

        public List<UserInformation> GetBranchesBasedOnProvince(int ProvinceId)
        {
            return db.CoreBranches.Where(x => x.ProvinceId == ProvinceId).Select(x => new UserInformation
            {
                BranchName = x.BranchName,
                Id = x.BranchId.ToString()

            }).ToList();
        }

        public static TeamLeaderInfomation GetTeamLeadersInfoNew(string userName)
        {
            return db.vw_GetTeamLeadersInformation.Where(x => x.EmailAddress == userName).Select(x => new TeamLeaderInfomation
            {
                CoreBranchId = x.BranchId,
                CoreProvinceId = x.ProvinceId,
                CoreTeamLeaderId = x.CoreTeamLeaderId,
                CoreUserId = x.CoreUserId

            }).FirstOrDefault();
        }
        #endregion

        #region Agent
        public static AgentInformation GetAgentInformation(string userName)
        {
            return db.vw_GetAgentsInformation.Where(x => x.EmailAddress == userName).Select(x => new AgentInformation
            {
                BranchName =  x.BranchName,
                DisplayName = x.FirstName + " " + x.LastName,
                Province = x.ProvinceName,
                TeamLeaderName = x.TeamLeadername

            }).FirstOrDefault();
        }
        #endregion

        #region Identity Helper Methods
        public static int GetCoreUserId()
        {
            return db.CoreUsers.Where(x => x.EmailAddress == UserName).FirstOrDefault().CoreUserId;
        }
        public static bool IsUserInRole(string roleName)
        {
            var q = (from a in db.CoreUsers
                     join b in db.CoreUserInTypes
                     on a.CoreUserId equals b.CoreUserId
                     join c in db.CoreUserTypes
                     on b.CoreUserTypeId equals c.CoreUserTypeId
                     where a.EmailAddress == UserName && c.UserTypeName == roleName
                     select a).Any();

            if(q)
            {
                return true;
            }
            return false;
        }
        #endregion
    }

   
}