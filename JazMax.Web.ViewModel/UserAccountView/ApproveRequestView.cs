using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class ApproveRequestView
    {
        public bool HasBeenApproved { get; set; } // HasBeenApproved
        public int ApprovedBy { get; set; } // ApprovedBy
        public string ApproverComments { get; set; } // ApproverComments
        public bool HasBeenCompleted { get; set; } // HasBeenCompleted
        public DateTime EffectiveDate { get; set; }
    }
}
