using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.WebJob.MoveBranchRequest
{
    public class MoveBranchCore
    {
        public  void DoWork()
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                var ListToMove = db.CoreUserBranchMoveRequests.Where(x => x.HasBeenApproved == true
                && x.EffectiveDate >= DateTime.Today && x.WebJobUpdateCompleted == false).ToList();

                foreach (var item in ListToMove)
                {
                    var Record = db.CoreUserBranchMoveRequests.Where(x => x.CoreUserMoveRequestId == item.CoreUserMoveRequestId)?.FirstOrDefault();
                    Record.WebJobUpdateCompleted = true;
                    db.SaveChanges();

                    var CoreUserTypeId = GetCoreUserType(Record.CoreUserId);

                    if (CoreUserTypeId > 0)
                    {

                        if (CoreUserTypeId == (int)JazMax.Common.Enum.UserType.Agent)
                        {
                            //Do Agent Move Logic
                            JazMax.BusinessLogic.UserAccounts.CoreUserService.MoveAgent(Record.CoreUserId, -1, Record.CoreBranchId);
                        }
                        else if (CoreUserTypeId == (int)JazMax.Common.Enum.UserType.TeamLeader)
                        {
                            //Do Teamleader Move Logic
                        }
                        else if (CoreUserTypeId == (int)JazMax.Common.Enum.UserType.PA)
                        {
                            //Not Implemented Yet
                        }
                    }
                    else
                    {
                        JazMax.BusinessLogic.AuditLog.ErrorLog.LogError(new JazMax.BusinessLogic.AuditLog.ErrorLog.ErrorMessage
                        {
                            CoreUserId = -1,
                            Message = "Invalid Core User Type",
                            Source = "JazMax.WebJob.MoveBranchRequest",
                            StackTrace = "N/A"
                        });
                    }
                }
            }
        }

        private int GetCoreUserType(int CoreUserId)
        {
            using (JazMax.DataAccess.JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                
                var t =  db.CoreUserInTypes.Where(x => x.CoreUserId == CoreUserId)?.Select(x =>x.CoreUserTypeId)?.FirstOrDefault();
                if (t > 0)
                {
                    return (int)t;
                }
                return -1;
            }
        }
    }
}
