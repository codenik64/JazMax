using System.Collections.Generic;
using System.Linq;
using JazMax.Core.SystemHelpers.Model;

namespace JazMax.Core.SystemHelpers
{
    public class JazMaxIdentityHelper
    {
        private static AzureDataAccess.JazMaxDBProdContext db = new AzureDataAccess.JazMaxDBProdContext();

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
                           Province = c.ProvinceName
                       };

            return user.FirstOrDefault();
        }
        #endregion

        #region Team Leader
        public static UserInformation GetTeamLeadersInfo(string userName)
        {
          
            return db.vw_GetTeamLeadersInformation.Where(x => x.EmailAddress == userName).Select(x => new UserInformation
            {


                BranchName = x.BranchName != null ? x.BranchName : "None",
                DisplayName = x.FirstName + " " + x.LastName,
                Id = x.CoreUserId.ToString(),
                Province = x.ProvinceName != null ? x.ProvinceName : "None"

            }).FirstOrDefault();
        }

        public List<UserInformation> GetTeamLeaderForProvince(int pId)
        {
            var q = from a in db.CoreUsers
                    join b in db.CoreTeamLeaders
                    on a.CoreUserId equals b.CoreUserId
                    where b.CoreProvinceId == pId
                    select new UserInformation
                    {
                        DisplayName = a.FirstName + " " + a.LastName,
                        Id = b.CoreTeamLeaderId.ToString()
                    };

            return q.ToList();
        }
        #endregion

        public List<UserInformation> GetBranchesBasedOnProvince(int ProvinceId)
        {
            return db.CoreBranches.Where(x => x.ProvinceId == ProvinceId).Select(x => new UserInformation
            {
                BranchName = x.BranchName,
                Id = x.BranchId.ToString()

            }).ToList();
        }
    }

   
}
