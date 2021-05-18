using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.Messenger
{
    public class MessageTemplate
    {
        public int MessengerTemplateId { get; set; } // MessengerTemplateId (Primary key)
        public int MessengerTemplateTypeId { get; set; } // MessengerTemplateTypeId
        public string TemplateName { get; set; } // TemplateName
        public int CoreBranchId { get; set; } // CoreBranchId
        public string TemplateHtml { get; set; } // TemplateHTML
        public int CoreUserId { get; set; } // CoreUserId
        public bool IsActive { get; set; } // IsActive
    }
}
