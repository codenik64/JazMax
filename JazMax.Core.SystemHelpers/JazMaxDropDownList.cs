using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JazMax.Core.SystemHelpers
{
    public class JazMaxDropDownList
    {
        private static JazMax.AzureDataAccess.JazMaxDBProdContext dbcon = new AzureDataAccess.JazMaxDBProdContext();

        public static IEnumerable<SelectListItem> GetUserTypes()
        {
            return dbcon.CoreUserTypes.Where(x => x.IsActive == true).Select(x => new SelectListItem
            {
                Text = x.UserTypeName,
                Value = x.CoreUserTypeId.ToString(),

            });
        }

        public static IEnumerable<SelectListItem> GetAllProvince()
        {
            return dbcon.CoreProvinces.Where(x => x.IsActive == true).Select(x => new SelectListItem
            {
                Text = x.ProvinceName,
                Value = x.ProvinceId.ToString(),

            });
        }

        public static IEnumerable<SelectListItem> GetProvinceNotAssigned()
        {
            return dbcon.CoreProvinces.Where(x => x.IsActive == true && x.IsAssigned == false).Select(x => new SelectListItem
            {
                Text = x.ProvinceName,
                Value = x.ProvinceId.ToString(),

            });
        }

        public static IEnumerable<SelectListItem> GetAllTeamLeaders()
        {
            var q = from a in dbcon.CoreUsers
                    join b in dbcon.CoreTeamLeaders
                    on a.CoreUserId equals b.CoreUserId
                    select new SelectListItem
                    {
                        Text = a.FirstName + " " + a.LastName,
                        Value = b.CoreTeamLeaderId.ToString()
                    };
            return q;
        }

        public static IEnumerable<SelectListItem> GetAllBranchesBasedOnProvince(int proId)
        {
            var q = from a in dbcon.CoreBranches
                    where a.ProvinceId == proId
                    select new SelectListItem
                    {
                        Text = a.BranchName,
                        Value = a.BranchId.ToString()
                    };
            return q;

        }


    }
}
