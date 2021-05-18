using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Core.Messenger;
using JazMax.Web.ViewModel.Messenger;
using JazMax.Core.Messenger.Messages;
using JazMax.Core.SystemHelpers;
using JazMax.Common.Enum;
namespace JazMax.Web.Areas.Messenger.Controllers
{
    public class MessageController : Controller
    {
        // GET: Messenger/Message
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyMessages()
        {
            return View();
        }
    
        #region Send Bulk Messing To System User
        public ActionResult SendUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendUser(SendMessage model)
        {
            JazMaxIdentityHelper.UserName = User.Identity.Name;
            model.CoreUserId = JazMaxIdentityHelper.GetCoreUserId();
            model.TriggerText = TriggerType.SysUsers.ToString();
            model.MessageType = 1;
            MessageCreationLogic.CreateTrigger(model);
            return RedirectToAction("Index" , "Home");

        }
        #endregion

        #region Send Bulk Messing To Lead Prospects
        public ActionResult SendProspect()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendProspect(SendMessage model)
        {
            JazMaxIdentityHelper.UserName = User.Identity.Name;
            model.CoreUserId = JazMaxIdentityHelper.GetCoreUserId();
            model.TriggerText = TriggerType.LeadProspects.ToString();
            model.MessageType = 1;
            MessageCreationLogic.CreateTrigger(model);
            return RedirectToAction("Index" , "Home");
        }

        public ActionResult GetMessageTemplate(int TemplateId)
        {
            JazMax.Core.Messenger.Service.MessengerTriggerLogic o = new Core.Messenger.Service.MessengerTriggerLogic();
            return Json(o.GetMessageTemplateBody(TemplateId));
        }
        #endregion
    }
}