using JazMax.DataAccess;
using JazMax.Web.ViewModel.UserAccountView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.BusinessLogic.UserAccounts
{
    public class CoreUserMoveRequestService
    {
        private static JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext();

        public List<UserBranchMoveView> GetAll()
        {
            var query = (from a in db.CoreUserBranchMoveRequests
                         join b in db.CoreUsers
                         on a.CoreUserId equals b.CoreUserId
                         join c in  db.CoreBranches
                         on a.CoreBranchId equals c.BranchId
                         where a.HasBeenCompleted == false
                         select new UserBranchMoveView
                         {
                             ApprovedBy = a.ApprovedBy,
                             ApprovedDate = a.ApprovedDate,
                             ApproverComments = a.ApproverComments,
                             HasBeenApproved = a.HasBeenApproved,
                             CoreBranchId = a.CoreBranchId,
                             CoreUserId = a.CoreUserId,
                             CoreUserMoveRequestId = a.CoreUserMoveRequestId,
                             HasBeenCompleted = a.HasBeenCompleted,
                             MoveRequestComment = a.MoveRequestComment,
                             RequestedDate = a.RequestedDate,
                         }).OrderByDescending(x =>x.RequestedDate).ToList();

            return query;
        }

        public void CaptureUserRequest(RequestBranchMoveView model)
        {
            try
            {
                CoreUserBranchMoveRequest table = new CoreUserBranchMoveRequest
                {
                    ApprovedBy = -1,
                    ApprovedDate = null,
                    ApproverComments = null,
                    HasBeenApproved = false,
                    CoreBranchId = model.CoreBranchId,
                    CoreUserId = model.CoreUserId,
                    EffectiveDate = null,
                    HasBeenCompleted = false,
                    MoveRequestComment = model.MoveRequestComment,
                    RequestedDate = DateTime.Now,
                    WebJobUpdateCompleted = false
                };
                db.CoreUserBranchMoveRequests.Add(table);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }

        public void CompleteUserRequest (ApproveRequestView model)
        {
            try
            {
                CoreUserBranchMoveRequest table = new CoreUserBranchMoveRequest
                {
                    ApprovedBy = model.ApprovedBy,
                    ApprovedDate = DateTime.Now,
                    ApproverComments = model.ApproverComments,
                    HasBeenApproved = model.HasBeenApproved,
                    EffectiveDate = model.EffectiveDate,
                    HasBeenCompleted = true,
                    WebJobUpdateCompleted = false
                };
                db.CoreUserBranchMoveRequests.Add(table);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                AuditLog.ErrorLog.LogError(e, 0);
            }
        }


    }
}
