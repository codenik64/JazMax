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
        private static JazMax.DataAccess.JazMaxDBProdContext dbcon = new DataAccess.JazMaxDBProdContext();

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

        public static IEnumerable<SelectListItem> GetAllBranches()
        {
            return dbcon.CoreBranches.Where(x => x.IsActive == true).Select(x => new SelectListItem
            {
                Text = x.BranchName,
                Value = x.BranchId.ToString(),

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

        public static IEnumerable<SelectListItem> GetAllTeamLeadersBasedOnPAProvince(int proId)
        {
            var q = from a in dbcon.CoreUsers
                    join b in dbcon.CoreTeamLeaders
                    on a.CoreUserId equals b.CoreUserId
                    where b.CoreProvinceId == proId
                    select new SelectListItem
                    {
                        Text = a.FirstName + " " + a.LastName,
                        Value = b.CoreTeamLeaderId.ToString()
                    };
            return q;
        }

        public static IEnumerable<SelectListItem> GetAllTeamLeadersThatAreNotAssigned(int proId)
        {
            List<int?> teamLeaderId = dbcon.CoreBranches.Where(x => x.ProvinceId == proId).Select(x => x.CoreTeamLeaderId).ToList();

            var q = from a in dbcon.CoreUsers
                    join b in dbcon.CoreTeamLeaders
                    on a.CoreUserId equals b.CoreUserId
                    where b.CoreProvinceId == proId &&
                    !teamLeaderId.Contains(b.CoreTeamLeaderId)
                    select new SelectListItem
                    {
                        Text = a.FirstName + " " + a.LastName,
                        Value = b.CoreTeamLeaderId.ToString()
                    };

            return q.ToList();

        }

        public static IEnumerable<SelectListItem> GetAllPropertyFeatures()
        {
            var q = from a in dbcon.PropertyFeatures
                    where a.IsFeatureActive == true
                    select new SelectListItem
                    {
                        Text = a.FeatureName,
                        Value = a.PropertyFeatureId.ToString()
                    };
            return q;
        }

        public static IEnumerable<SelectListItem> GetAllPropertyTypes()
        {
            var q = from a in dbcon.PropertyTypes
                    where a.IsActive == true
                    select new SelectListItem
                    {
                        Text = a.TypeName,
                        Value = a.PropertyTypeId.ToString()
                    };
            return q;
        }

        //Gets a list of agents for a branch
        //Text - Core User Name
        //Value - CoreAgentId
        public static IEnumerable<SelectListItem> GetAgentsForBranch(int BranchId)
        {
            var q = from a in dbcon.CoreUsers
                    join b in  dbcon.CoreAgents
                    on a.CoreUserId equals b.CoreUserId
                    join c in dbcon.CoreBranches
                    on b.CoreBranchId equals c.BranchId
                    where c.BranchId == BranchId
                    select new SelectListItem
                    {
                        Text = a.FirstName + " " + a.LastName,
                        Value = b.CoreAgentId.ToString()
                    };
            return q;
        }

        public static IEnumerable<SelectListItem> GetAllPropertyPriceTypes()
        {
            return (from a in dbcon.PropertyListingPricingTypes
                    where a.IsActive == true
                    select new SelectListItem
                    {
                        Text = a.TypeName,
                        Value = a.PropertyListingPricingTypeId.ToString()
                    });
        }

        public static IEnumerable<SelectListItem> GetGender()
        {

            List<SelectListItem> listItems = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Male",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "Female",
                    Value = "2",
                }
            };

            return listItems;
        }







    }
}
