using System.Collections.Generic;
using System.Linq;

namespace JazMax.Core.SystemHelpers
{
    public class JazMaxIdentityHelper
    {
        private static AzureDataAccess.JazMaxDBProdContext db = new AzureDataAccess.JazMaxDBProdContext();


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

        public static UserInformation GetTeamLeaderUserInformation(string userName)
        {
            var user = from a in db.CoreUsers
                       join b in db.CoreTeamLeaders
                       on a.CoreUserId equals b.CoreUserId
                       join c in db.CoreProvinces
                       on b.CoreProvinceId equals c.ProvinceId
                       where a.EmailAddress == userName
                       select new UserInformation
                       {
                           DisplayName = a.FirstName + " " + a.LastName,
                           Province = c.ProvinceName
                       };

            return user.FirstOrDefault();
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

    }

    public class UserInformation
    {
        public string DisplayName { get; set; }
        public string Province { get; set; }
        public string BranchName { get; set; }
        public string Id { get; set; }
    }
}
