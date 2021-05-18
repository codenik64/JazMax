using System.Collections.Generic;
using System.Linq;
using JazMax.Core.SystemHelpers.Model;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System;
using JazMax.DataAccess;

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

        public static int GetAgentId()
        {
            var query = (from t in db.CoreAgents
                         join b in db.CoreUsers
                         on t.CoreUserId equals b.CoreUserId
                         where b.EmailAddress == UserName
                         select t.CoreAgentId).FirstOrDefault();
            return query;
        }

        public static List<int> GetPAProvinceIdList()
        {
            List<int> user = (from a in db.CoreUsers
                              join b in db.CorePas
                              on a.CoreUserId equals b.CoreUserId
                              join c in db.CoreProvinces
                              on b.ProvinceId equals c.ProvinceId
                              join d in db.CoreBranches
                              on c.ProvinceId equals d.ProvinceId
                              where a.EmailAddress == UserName
                              select d.BranchId).ToList();

            return user;

        }

        public static User.UserData GetUserInformationNew()
        {
            var RoleName = (from t in db.CoreUsers
                            join b in db.CoreUserInTypes
                            on t.CoreUserId equals b.CoreUserId
                            join c in db.CoreUserTypes
                            on b.CoreUserTypeId equals c.CoreUserTypeId
                            where t.EmailAddress == UserName
                            select c.UserTypeName)?.FirstOrDefault();

            if (RoleName != null)
            {
                if (RoleName == JazMax.Common.Enum.UserType.Agent.ToString())
                {
                    if (IsUserInRole(RoleName))
                    {
                        return db.VwGetAgentsInformations.Where(x => x.EmailAddress == UserName).Select(x => new User.UserData
                        {
                            BranchId = x.BranchId,
                            AgentId = x.CoreAgentId,
                            CoreUserId = x.CoreUserId,
                            ProvinceId = x.ProvinceId,
                            TeamLeaderId = x.CoreTeamLeaderId != null ? (int)x.CoreTeamLeaderId : 0

                        })?.FirstOrDefault();
                    }
                }
                else if (RoleName == JazMax.Common.Enum.UserType.TeamLeader.ToString())
                {
                    if (IsUserInRole(RoleName))
                    {
                        var q = db.VwGetTeamLeadersInformations.Where(x => x.EmailAddress == UserName).Select(x => new User.UserData
                        {
                            ProvinceId = x.ProvinceId != null ? (int)x.ProvinceId : 0,
                            AgentId = 0,
                            BranchId = x.BranchId != null ? (int)x.BranchId : 0,
                            CoreUserId = x.CoreUserId != null ? (int)x.CoreUserId : 0,
                            TeamLeaderId = x.CoreTeamLeaderId != null ? (int)x.CoreTeamLeaderId : 0

                        })?.FirstOrDefault();
                    }
                }
                else if (RoleName == JazMax.Common.Enum.UserType.PA.ToString())
                {
                    if (IsUserInRole(RoleName))
                    {
                        var user = from a in db.CoreUsers
                                   join b in db.CorePas
                                   on a.CoreUserId equals b.CoreUserId
                                   join c in db.CoreProvinces
                                   on b.ProvinceId equals c.ProvinceId
                                   where a.EmailAddress == UserName
                                   select new User.UserData
                                   {
                                       ProvinceId = (int)b.ProvinceId
                                   };

                        return user?.FirstOrDefault();
                    }
                }
                else if (RoleName == JazMax.Common.Enum.UserType.CEO.ToString())
                {
                    return new User.UserData
                    {
                        AgentId = 0,
                        CoreUserId = 0,
                        BranchId = 0,
                        ProvinceId = 0,
                        TeamLeaderId = 0,
                    };
                }
            }
            return new User.UserData
            {
                AgentId = 0,
                CoreUserId = 0,
                BranchId = 0,
                ProvinceId = 0,
                TeamLeaderId = 0,
            };
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
            var q = db.VwGetTeamLeadersInformations.Where(x => x.EmailAddress == userName).Select(x => new UserInformation
            {
                BranchName = x.BranchName != null ? x.BranchName : "None",
                DisplayName = x.FirstName + " " + x.LastName != null ? x.FirstName + " " + x.LastName : "None",
                Id = x.CoreUserId.ToString(),
                Province = x.ProvinceName != null ? x.ProvinceName : "None"

            })?.FirstOrDefault();

            if (q == null)
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
            cmd.CommandText = @"SELECT CoreTeamLeaderId FROM CoreTeamLeader WHERE CoreProvinceId =" + pId + @"AND CoreTeamLeaderId 
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
            return db.CoreBranches.Where(x => x.ProvinceId == ProvinceId && x.IsActive == true).Select(x => new UserInformation
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
            return db.VwGetAgentsInformations.Where(x => x.EmailAddress == userName).Select(x => new AgentInformation
            {
                BranchName = x.BranchName,
                DisplayName = x.FirstName + " " + x.LastName,
                Province = x.ProvinceName,
                TeamLeaderName = "NA",
                BranchId = x.BranchId

            })?.FirstOrDefault();
        }

        public static AgentInformation GetAgentInformationNew()
        {
            return db.VwGetAgentsInformations.Where(x => x.EmailAddress == UserName).Select(x => new AgentInformation
            {
                BranchName = x.BranchName,
                DisplayName = x.FirstName + " " + x.LastName,
                Province = x.ProvinceName,
                TeamLeaderName = "NA",
                BranchId = x.BranchId

            })?.FirstOrDefault();
        }
        #endregion

        #region Identity Helper Methods
        public static int GetCoreUserId()
        {
            try
            {
                return (int)db.CoreUsers.Where(x => x.EmailAddress == UserName)?.FirstOrDefault()?.CoreUserId;
            }
            catch
            {
                return 0;
            }
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

            if (q)
            {
                return true;
            }
            return false;
        }

        public static CoreUserEmailData User()
        {
            using (JazMaxDBProdContext db = new JazMaxDBProdContext())
            {
                var user = (from t in db.CoreUsers
                            join b in db.CoreAgents
                            on t.CoreUserId equals b.CoreUserId
                            join c in db.CoreBranches
                            on b.CoreBranchId equals c.BranchId
                            where t.IsActive == true
                            select new CoreUserEmailData
                            {
                                BranchId = (int)b.CoreBranchId,
                                CoreUserId = t.CoreUserId,
                                CoreUserTypeId = 4,
                                ProvinceId = (int)c.ProvinceId,
                                Email = t.EmailAddress,
                                Name = t.FirstName + " " + t.LastName
                            }).Union(from t in db.CoreUsers
                                     join b in db.CoreTeamLeaders
                                     on t.CoreUserId equals b.CoreUserId
                                     join c in db.CoreBranches
                                     on b.CoreTeamLeaderId equals c.CoreTeamLeaderId
                                     where t.IsActive == true
                                     select new CoreUserEmailData
                                     {
                                         BranchId = (int)c.BranchId,
                                         CoreUserId = t.CoreUserId,
                                         CoreUserTypeId = 3,
                                         ProvinceId = (int)c.ProvinceId,
                                         Email = t.EmailAddress,
                                         Name = t.FirstName + " " + t.LastName
                                     }).Union(from t in db.CoreUsers
                                              join b in db.CorePas
                                              on t.CoreUserId equals b.CoreUserId
                                              where t.IsActive == true
                                              select new CoreUserEmailData
                                              {
                                                  BranchId = 0,
                                                  CoreUserId = t.CoreUserId,
                                                  CoreUserTypeId = 5,
                                                  ProvinceId = (int)b.ProvinceId,
                                                  Email = t.EmailAddress,
                                                  Name = t.FirstName + " " + t.LastName
                                              }).Where(x => x.Email == UserName)?.FirstOrDefault();

                if (user == null)
                {
                    return new CoreUserEmailData
                    {
                        BranchId = 0,
                        CoreUserId = 0,
                        CoreUserTypeId = 0,
                        Email = null,
                        Name = null,
                        ProvinceId = 0
                    };
                }


                return user;
            }
        }

        public class CoreUserEmailData
        {
            public int CoreUserId { get; set; }
            public int BranchId { get; set; }
            public int ProvinceId { get; set; }
            public int CoreUserTypeId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public static bool IsUserAccountActive()
        {
            bool? query = (from t in db.CoreUsers
                           where t.EmailAddress == UserName
                           select t.IsActive).FirstOrDefault();

            if (query == null)
            {
                return false;
            }

            if ((bool)query)
            {
                return true;
            }
            return false;
        }
        #endregion
    }


}
