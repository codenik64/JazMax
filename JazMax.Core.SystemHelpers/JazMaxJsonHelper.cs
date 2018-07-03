using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JazMax.Core.SystemHelpers
{
    public class JazMaxJsonHelper
    {
        public string Result { get; set; }
        public string Message { get; set; }
        public JsonRequestBehavior AllowGet { get; set; }
        public string Data { get; set; }

        public JazMaxJsonHelper()
        {
            AllowGet = JsonRequestBehavior.AllowGet;
        }
    }

    public class JazMaxMultipleResultHelper
    {
        public string Result { get; set; }
        public string Message { get; set; }
        public JsonRequestBehavior AllowGet { get; set; }
        public string Data { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }

        public JazMaxMultipleResultHelper()
        {
            AllowGet = JsonRequestBehavior.AllowGet;
        }
    }

}
