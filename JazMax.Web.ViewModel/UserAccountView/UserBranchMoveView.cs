using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class UserBranchMoveView
    {
        public int CoreUserMoveRequestId { get; set; } // CoreUserMoveRequestId (Primary key)
        public int CoreUserId { get; set; } // CoreUserId
        public int CoreBranchId { get; set; } // CoreBranchId
        public string MoveRequestComment { get; set; } // MoveRequestComment
        public System.DateTime RequestedDate { get; set; } // RequestedDate
        public bool HasBeenApproved { get; set; } // HasBeenApproved
        public int ApprovedBy { get; set; } // ApprovedBy
        public System.DateTime? ApprovedDate { get; set; } // ApprovedDate
        public string ApproverComments { get; set; } // ApproverComments
        public bool HasBeenCompleted { get; set; } // HasBeenCompleted
        public CoreUserDetails UserDetails { get; set; }
    }
}
