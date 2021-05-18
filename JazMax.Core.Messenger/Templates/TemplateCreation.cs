using JazMax.DataAccess;
using JazMax.Web.ViewModel.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazMax.Core.Messenger.Templates
{
    public class TemplateCreation
    {
        public static void CreateTemplate(MessageTemplate model)
        {
            using (JazMaxDBProdContext db = new DataAccess.JazMaxDBProdContext())
            {
                MessengerTemplate table = new MessengerTemplate()
                {
                    CoreBranchId = model.CoreBranchId,
                    CoreUserId = model.CoreUserId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    MessengerTemplateTypeId = 1,
                    TemplateHtml = model.TemplateHtml,
                    TemplateName = model.TemplateName
                };
                db.MessengerTemplates.Add(table);
                db.SaveChanges();
            }
        }
    }
}
