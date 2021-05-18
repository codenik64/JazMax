using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using JazMax.Web.ViewModel.UserAccountView;

namespace JazMax.Core.SystemHelpers
{
    public static class JazMaxMvcExtensions
    {
        public static MvcHtmlString JazMaxUserDetails(this HtmlHelper htmlHelper, string linkText, object routeValues)
        {
            if (linkText == "")
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, "Details", "User", routeValues, null);
        }

        public static MvcHtmlString JazMaxActionLink(this HtmlHelper htmlHelper, string linkText)
        {
            if (linkText == "")
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, JazMaxHTML.View, JazMaxHTML.Controller, JazMaxHTML.PrimaryKey, null);
        }

        public static MvcHtmlString JazMaxProvinceDetails(this HtmlHelper htmlHelper, string linkText, object routeValues)
        {
            if (linkText == "")
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, "Details", "Province", routeValues, null);
        }

        public static MvcHtmlString JazMaxBranchDetails(this HtmlHelper htmlHelper, string linkText, object routeValues)
        {
            if (linkText == "")
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, "Details", "Branch", routeValues, null);
        }

        public static string DrawTable(CoreUserDetails model)
        {
            string Controller = "User";
            string View = "Details";
            int Key = model.CoreUserId;
            string table1 = "<tr>";
            string str1 = @"<td><a href=/" + Controller + "/" + View + "/" + model.CoreUserId + ">" + model.FirstName + "</a></td>";
            string str2 = @"<td><a href=/User/Details/" + model.CoreUserId + ">" + model.MiddleName + "</a></td>";
            string str3 = @"<td><a href=/User/Details/" + model.CoreUserId + ">" + model.LastName + "</a></td>";
            string str4 = @"<td><a href=/User/Details/" + model.CoreUserId + ">" + model.IDNumber + "</a></td>";
            string str5 = @"<td><a href=/User/Details/" + model.CoreUserId + ">" + model.PhoneNumber + "</a></td>";
            string str6 = @"<td><a href=/User/Details/" + model.CoreUserId + ">" + model.CellPhone + "</a></td>";
            string str7 = @"<td><a href=/User/Details/" + model.CoreUserId + ">" + model.EmailAddress + "</a></td>";
            string str8 = @"<td><a href=/User/Details/" + model.CoreUserId + ">" + model.GenderId + "</a></td>";
            string str9 = @"<td><a href=/User/Details/" + model.CoreUserId + ">" + model.UserType + "</a></td>";
            string table2 = "</tr>";

            StringBuilder sb = new StringBuilder();
            sb.Append(table1);
            sb.Append(str1);
            sb.Append(str2);
            sb.Append(str3);
            sb.Append(str4);
            sb.Append(str5);
            sb.Append(str6);
            sb.Append(str7);
            sb.Append(str8);
            sb.Append(str9);
            sb.Append(table2);

            return sb.ToString();

        }

        public static string DrawPanel(JazMaxPanel model)
        {
            string a = "<div id=" + model.PanelId + ">";

            if (model.isHidden)
            {
                a = "<div id=" + model.PanelId + " " + "hidden" + ">";
            }

            string str1 = "";
            if (model.PanelType == PanelType.Success)
            {
                str1 = "<div class='alert alert-success'>";
            }
            if (model.PanelType == PanelType.Info)
            {
                str1 = "<div class='alert alert-info'>";
            }
            if (model.PanelType == PanelType.Warning)
            {
                str1 = "<div class='alert alert-warning'>";
            }
            if (model.PanelType == PanelType.Danger)
            {
                str1 = "<div class='alert alert-danger'>";
            }


            string str2 = "<p>" + model.Message + "</p> </div> </div>";
            StringBuilder sb = new StringBuilder();
            sb.Append(a);
            sb.Append(str1);
            sb.Append(str2);
            return sb.ToString();

        }

        public static string JaxMaxBootstrapTable(JazMaxTable table)
        {
            string script = "<script>";
            string scripta = " $('#" + table.TableId + "').DataTable();";
            string scriptc = "</script>";

            string str1 = "     <table id='" + table.TableId + "' class='table table-striped table - bordered dt - responsive nowrap' width='100 %' cellspacing='0'>";
            StringBuilder sb = new StringBuilder();
            //sb.Append(script);
            //sb.Append(scripta);
            //sb.Append(scriptc);
            sb.Append(str1);
            return sb.ToString();
        }
    }

    public static class JazMaxHTML
    {
        public static string Controller { get; set; }
        public static string View { get; set; }
        public static object PrimaryKey { get; set; }
    }

    public enum PanelType
    {
        Success,
        Info,
        Warning,
        Danger
    }

    public class JazMaxPanel
    {
        public string PanelId { get; set; }
        public bool isHidden { get; set; }
        public PanelType PanelType { get; set; }
        public string Message { get; set; }
    }

    public class JazMaxTable
    {
        public string TableId { get; set; }
    }
}
