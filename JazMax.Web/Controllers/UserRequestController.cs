using JazMax.BusinessLogic.UserAccounts;
using JazMax.Core.SystemHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JazMax.Web.Controllers
{
    public class UserRequestController : Controller
    {
        private static CoreUserMoveRequestService obj = new CoreUserMoveRequestService();
        // GET: UserRequest
        public ActionResult Index()
        {
            return View(obj.GetAll());
        }

        public ActionResult Capture()
        {
            return View();
        }

        public ActionResult SaveRequest(string CoreBranchId, string MoveRequestComment)
        {
            try
            {
                JazMaxIdentityHelper.UserName = User.Identity.Name;
                obj.CaptureUserRequest(new ViewModel.UserAccountView.RequestBranchMoveView
                {
                    CoreBranchId = Convert.ToInt32(CoreBranchId),
                    MoveRequestComment = MoveRequestComment,
                    CoreUserId = JazMaxIdentityHelper.GetCoreUserId()
                });

                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Success,
                    Message = Common.Models.JsonMessage.Saved,
                });
            }
            catch
            {
                return Json(new JazMaxJsonHelper
                {
                    Result = Common.Models.JsonResult.Error,
                    Message = Common.Models.JsonMessage.Error,
                });
            }
        }

        public ActionResult MyRequests()
        {
            JazMaxIdentityHelper.UserName = User.Identity.Name;
            return View();
        }

    }
}