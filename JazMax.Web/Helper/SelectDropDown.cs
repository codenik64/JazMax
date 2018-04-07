using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Helper
{
    public class SelectDropDown
    {
        //private static JazMax.AzureDataAccess.JazMaxDBProdContext dbcon = new AzureDataAccess.JazMaxDBProdContext();

        //public static IEnumerable<SelectListItem> GetUserTypes()
        //{
        //    return dbcon.CoreUserTypes.Where(x =>x.IsActive == true).Select(x => new SelectListItem
        //    {
        //        Text = x.UserTypeName,
        //        Value = x.CoreUserTypeId.ToString(),
     
        //    });
        //}

        //public int GetCurrentCoreUserId(string userName)
        //{
        //    var query = from x in dbcon.CoreUsers
        //                join t in dbcon.AspNetUsers
        //                on x.EmailAddress equals t.Email
        //                where t.Email == userName
        //                select x.CoreUserId;

        //    return query.FirstOrDefault();
        //}

        //public string GetAspUserRoleName(int CoreUserTypeId)
        //{
        //    var query = from t in dbcon.CoreUserTypes
        //                join m in dbcon.AspNetRoles
        //                on t.UserTypeName equals m.Name
        //                where t.CoreUserTypeId == CoreUserTypeId
        //                select m.Id;

        //    return query.FirstOrDefault();
        //}
    }
}