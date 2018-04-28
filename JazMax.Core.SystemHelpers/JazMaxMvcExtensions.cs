using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace JazMax.Core.SystemHelpers
{
    public static class JazMaxMvcExtensions
    {
        public static MvcHtmlString JazMaxUserDetails(this HtmlHelper htmlHelper, string linkText, object routeValues)
        {
            if(linkText == "")
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, "Details", "User", routeValues, null);
        }

       

    }
}
