using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace JazMax.Core.SystemHelpers
{
    public class JazMaxControllerCore : Controller
    {
        public static string RenderPartialToString(string controlName, object viewData)
        {
            ViewPage viewPage = new ViewPage() { ViewContext = new ViewContext() };

            viewPage.ViewData = new ViewDataDictionary(viewData);
            viewPage.Controls.Add(viewPage.LoadControl(controlName));

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter tw = new HtmlTextWriter(sw))
                {
                    viewPage.RenderControl(tw);
                }
            }
            return sb.ToString();
        }

        public static PartialViewResult JazMaxPartialView()
        {
            return new PartialViewResult();
        }

        public ActionResult JazMaxView()
        {
            return View();
        }

    }
}
