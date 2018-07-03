using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Web.ViewModel.UserAccountView
{
    public class RequestBranchMoveView
    {
        public int CoreUserMoveRequestId { get; set; } // CoreUserMoveRequestId (Primary key)
        public int CoreUserId { get; set; } // CoreUserId
        [Display(Name ="Branch")]
        public int CoreBranchId { get; set; } // CoreBranchId
        [Display(Name = "Comment")]
        public string MoveRequestComment { get; set; } // MoveRequestComment
    }
}
