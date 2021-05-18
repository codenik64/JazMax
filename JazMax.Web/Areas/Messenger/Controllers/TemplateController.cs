using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JazMax.Web.ViewModel.Messenger;
using JazMax.Core.Messenger.Messages;
using JazMax.Core.SystemHelpers;
using JazMax.Common.Enum;
using JazMax.Core.Messenger.Templates;
namespace JazMax.Web.Areas.Messenger.Controllers
{
    public class TemplateController : Controller
    {
        // GET: Messenger/Template
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(MessageTemplate model)
        {
            JazMaxIdentityHelper.UserName = User.Identity.Name;
            model.CoreUserId = JazMaxIdentityHelper.GetCoreUserId();
            if (!JazMaxIdentityHelper.IsUserInRole("CEO,PA"))
            {
                model.CoreBranchId = JazMaxIdentityHelper.User().BranchId;
            }

            TemplateCreation.CreateTemplate(model);
            return View();
        }

    }
}