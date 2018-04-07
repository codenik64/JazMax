using System.Collections.Generic;
using System.Linq;
using JazMax.Core.SystemHelpers.Model;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System;

namespace JazMax.Core.SystemHelpers
{
    public class JazMaxIdentityHelper
    {
        private static DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();
        SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["JazMaxDBProdContextA"].ToString());
        public static string UserName { get; set; }

        public static UserInformation GetBasicUserInfo()
        {
            var query = (from t in db.CoreUsers
                        where t.EmailAddress == UserName
                        select new UserInformation
                        {
                            DisplayName = t.FirstName + " " + t.LastName,
                        }).FirstOrDefault();

            return query;
        }

        #region Personal Assisstant
        public static UserInformation GetPAUserInformation(string userName)
        {
            var user = from a in db.CoreUsers
                       join b in db.CorePas
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
            var q =  db.VwGetTeamLeadersInformations.Where(x => x.EmailAddress == userName).Select(x => new UserInformation
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
            List<int?> teamLeaderId = new List<int?>();

            #region SQL Command
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            cmd.CommandText = @"SELECT CoreTeamLeaderId FROM CoreTeamLeader WHERE CoreProvinceId ="+ pId  + @"AND CoreTeamLeaderId 
                               NOT IN (SELECT CoreTeamLeaderId FROM CoreBranch)";
            
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;
            sqlConnection1.Open();
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    teamLeaderId.Add(Convert.ToInt32(reader[0]));
                }
            }
            sqlConnection1.Close();
            #endregion

            var q = from a in db.CoreUsers
                    join b in db.CoreTeamLeaders
                    on a.CoreUserId equals b.CoreUserId
                    where 
                    teamLeaderId.Contains(b.CoreTeamLeaderId)
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

        public static TeamLeaderInfomation GetTeamLeadersInfoNew()
        {
            return db.VwGetTeamLeadersInformations.Where(x => x.EmailAddress == UserName).Select(x => new TeamLeaderInfomation
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
            return db.VwGetTeamLeadersInformations.Where(x => x.EmailAddress == userName).Select(x => new AgentInformation
            {
                BranchName =  x.BranchName,
                DisplayName = x.FirstName + " " + x.LastName,
                Province = x.ProvinceName,
                TeamLeaderName = "NA"

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
            List<string> SeperatedString = roleName.Split(',').ToList();
            var q = (from a in db.CoreUsers
                     join b in db.CoreUserInTypes
                     on a.CoreUserId equals b.CoreUserId
                     join c in db.CoreUserTypes
                     on b.CoreUserTypeId equals c.CoreUserTypeId
                     where a.EmailAddress == UserName && SeperatedString.Contains(c.UserTypeName)
                     select a).Any();

            if(q)
            {
                return true;
            }
            return false;
        }

        public static bool IsUserAccountActive()
        {
            bool? query = (from t in db.CoreUsers
                          where t.EmailAddress == UserName
                          select t.IsActive).FirstOrDefault();

            if(query == null)
            {
                return false;
            }

            if((bool)query)
            {
                return true;
            }
            return false;
        }
        #endregion
    }

   
}
