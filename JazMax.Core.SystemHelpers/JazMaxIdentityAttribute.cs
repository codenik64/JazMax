using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace JazMax.Core.SystemHelpers
{
    public class JazMaxIdentityAttribute : AuthorizeAttribute
    {
        public string UserGroup { get; set; }

        //TO DO: Force For CEO
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();

            List<string> SeperatedString = UserGroup.Split(',').ToList();

            var query = (from a in db.CoreUsers
                         join b in db.CoreUserInTypes
                         on a.CoreUserId equals b.CoreUserId
                         join c in db.CoreUserTypes
                         on b.CoreUserTypeId equals c.CoreUserTypeId
                         where SeperatedString.Contains(c.UserTypeName)
                         && a.EmailAddress == httpContext.User.Identity.Name
                         select a).Any();


            if (query)
            {
                return true;
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary(
                   new
                   {
                       controller = "Error",
                       action = "Unauthorised"
                   })
               );
        }
    }
}
